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
            
            try
            {
                var dialogParticipants = await _dialogRepository.GetDialogMembers(message.DialogId);
                var dialogParticipantsWithoutSender = dialogParticipants.Where(x => x != message.Sender).ToList();
                    
                var participants = dialogParticipantsWithoutSender.Where(login=> !_cacheService.CheckDialogIsOpened(login, message.DialogId)).ToList();
                
                message.DateTimeUtc = DateTime.UtcNow;
                var messageEntity = message.ToEntity(participants);
                await _messageRepository.AddMessage(messageEntity);

                _dialogRepository.UpdateLastMessageDate(
                    new UpdateLastMessageDateParameter()
                    {
                        DialogId = message.DialogId,
                        Date = messageEntity.DateTimeUtc
                    });

                var members = await _dialogRepository.GetDialogMembers(message.DialogId);
                var clientIds = _cacheService.GetConnectionIdsByLogins(members).ToList();

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

        [HubMethodName("setOpenedDialog")]
        public BaseResponse SetOpenedDialog(string dialogId)
        {
            var connectionId = Context.ConnectionId;
            var isSucces = _cacheService.SetOpenedDialog(connectionId, dialogId);
            return new BaseResponse() {Success = isSucces};
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
