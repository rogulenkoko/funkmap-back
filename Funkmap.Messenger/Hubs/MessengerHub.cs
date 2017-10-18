using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Funkmap.Auth.Contracts.Models;
using Funkmap.Common.Filters;
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
    [ValidateRequestModel]
    public class MessengerHub : Hub, IMessengerHub
    {
        private readonly IMessengerConnectionService _connectionService;
        private readonly IDialogRepository _dialogRepository;
        private readonly IMessageRepository _messageRepository;

        public MessengerHub(IMessengerConnectionService connectionService,
                            IDialogRepository dialogRepository,
                            IMessageRepository messageRepository)
        {
            _connectionService = connectionService;
            _dialogRepository = dialogRepository;
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
                    
                var participants = dialogParticipantsWithoutSender.Where(login=> !_connectionService.CheckDialogIsOpened(login, message.DialogId)).ToList();
                if (participants.Count == dialogParticipants.Count - 1)
                {
                    message.IsNew = true;
                }
                
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
                var clientIds = _connectionService.GetConnectionIdsByLogins(members).ToList();

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
        public async Task<BaseResponse> SetOpenedDialog(string dialogId)
        {
            if (String.IsNullOrEmpty(dialogId)) return new BaseResponse() {Success = false};
            var connectionId = Context.ConnectionId;
            var isSucces = _connectionService.SetOpenedDialog(connectionId, dialogId);

            var dialogMembers = await _dialogRepository.GetDialogMembers(dialogId);
            var login = Context.QueryString["login"];
            var userConnections = _connectionService.GetConnectionIdsByLogins(new List<string>() {login});
            var connectionIds = _connectionService.GetConnectionIdsByLogins(dialogMembers).Except(userConnections).ToList();

            Clients.Clients(connectionIds).onDialogRead(dialogId);

            return new BaseResponse() {Success = isSucces};
        }

        public override Task OnConnected()
        {
            var connectionId = Context.ConnectionId;
            var login = Context.QueryString["login"];

            _connectionService.AddOnlineUser(connectionId, login);
            Clients.All.onUserConnected(login);

            return base.OnConnected();
        }


        public override Task OnDisconnected(bool stopCalled)
        {
            
            var connectionId = Context.ConnectionId;
            string login;
            _connectionService.RemoveOnlineUser(connectionId, out login);
            Clients.All.onUserDisconnected(login);

            return base.OnDisconnected(stopCalled);
        }
    }
}
