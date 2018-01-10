using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Funkmap.Common.Cqrs.Abstract;
using Funkmap.Common.Filters;
using Funkmap.Common.Models;
using Funkmap.Messenger.Command.Commands;
using Funkmap.Messenger.Data.Parameters;
using Funkmap.Messenger.Data.Repositories.Abstract;
using Funkmap.Messenger.Hubs.Abstract;
using Funkmap.Messenger.Mappers;
using Funkmap.Messenger.Models;
using Funkmap.Messenger.Services;
using Funkmap.Messenger.Services.Abstract;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace Funkmap.Messenger.Hubs
{
    [HubName("messenger")]
    [ValidateRequestModel]
    public class MessengerHub : Hub<IMessengerHub>
    {
        private readonly IMessengerConnectionService _connectionService;
        private readonly IDialogRepository _dialogRepository;
        private readonly ICommandBus _commandBus;

        public MessengerHub(IMessengerConnectionService connectionService,
                            IDialogRepository dialogRepository,
                            ICommandBus commandBus)
        {
            _connectionService = connectionService;
            _dialogRepository = dialogRepository;
            _commandBus = commandBus;
        }

        [HubMethodName("sendMessage")]
        public BaseResponse SendMessage(Message message)
        {
            var response = new BaseResponse();
            
            try
            {

                var usersWithOpenedCurrentDialog = _connectionService.GetDialogParticipants(message.DialogId);
                var command = new SaveMessageCommand()
                {
                    DialogId = message.DialogId,
                    Sender = message.Sender,
                    Text = message.Text,
                    Content = message.Images?.ToList(),//todo
                    UsersWithOpenedCurrentDialog = usersWithOpenedCurrentDialog
                };
                _commandBus.Execute<SaveMessageCommand>(command);
                
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

            Clients.Clients(connectionIds).OnDialogRead(dialogId);

            return new BaseResponse() {Success = isSucces};
        }

        public override Task OnConnected()
        {
            var connectionId = Context.ConnectionId;
            var login = Context.QueryString["login"];

            _connectionService.AddOnlineUser(connectionId, login);
            Clients.All.OnUserConnected(login);

            return base.OnConnected();
        }


        public override Task OnDisconnected(bool stopCalled)
        {
            
            var connectionId = Context.ConnectionId;
            string login;
            _connectionService.RemoveOnlineUser(connectionId, out login);
            Clients.All.OnUserDisconnected(login);

            return base.OnDisconnected(stopCalled);
        }
    }
}
