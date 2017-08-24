using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Funkmap.Auth.Contracts.Models;
using Funkmap.Common.Models;
using Funkmap.Messenger.Data.Entities;
using Funkmap.Messenger.Data.Parameters;
using Funkmap.Messenger.Data.Repositories.Abstract;
using Funkmap.Messenger.Mappers;
using Funkmap.Messenger.Models;
using Funkmap.Messenger.Services;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace Funkmap.Messenger.Hubs
{
    [HubName("messenger")]
    public class MessengerHub : Hub, IMessengerHub
    {
        private readonly IMessengerCacheService _cacheService;
        private readonly IDialogRepository _dialogRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly UserService _userService;

        public MessengerHub(IMessengerCacheService cacheService,
                            IDialogRepository dialogRepository,
                            IMessageRepository messageRepository,
                            UserService userService)
        {
            _cacheService = cacheService;
            _dialogRepository = dialogRepository;
            _userService = userService;
            _messageRepository = messageRepository;
        }

        [HubMethodName("sendMessage")]
        public async Task<BaseResponse> SendMessage(Message message)
        {
            var response = new BaseResponse();

            var members = await _dialogRepository.GetDialogMembers(message.DialogId);
            var clientIds = _cacheService.GetConnectionIdsByLogins(members).ToList();

            try
            {
                List<string> participants;
                if (message.IsInNewDialog)
                {
                    var dialogEntity = new DialogEntity()
                    {
                        LastMessageDate = DateTime.UtcNow,
                        Participants = new List<string>() { message.Sender, message.Reciever }
                    };
                    var dialogId = await _dialogRepository.CreateAsync(dialogEntity);
                    message.DialogId = dialogId.ToString();
                    participants = new List<string>() {message.Reciever};
                }
                else
                {
                    var dialogParticipants = await _dialogRepository.GetDialogMembers(message.DialogId);
                    participants = dialogParticipants.ToList();
                }

                var messageEntity = message.ToEntity(participants);
                await _messageRepository.AddMessage(messageEntity);

                _dialogRepository.UpdateLastMessageDate(
                    new UpdateLastMessageDateParameter()
                    {
                        DialogId = message.DialogId,
                        Date = messageEntity.DateTimeUtc
                    });

                Clients.Clients(clientIds).OnMessageSent(message);
                response.Success = true;
                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new BaseResponse();
            }
        } 

        public override Task OnConnected()
        {
            var connectionId = Context.ConnectionId;
            var login = Context.QueryString["login"];

            _cacheService.AddOnlineUser(connectionId, login);
            Clients.All.onUserConnected(login);

            return base.OnConnected();
        }


        public override Task OnDisconnected(bool stopCalled)
        {
            
            var connectionId = Context.ConnectionId;
            string login;
            _cacheService.RemoveOnlineUser(connectionId, out login);
            Clients.All.onUserDisconnected(login);

            _userService.UpdateLastVisitDate(new UserUpdateLastVisitDateRequest() { Login = login });

            return base.OnDisconnected(stopCalled);
        }
    }
}
