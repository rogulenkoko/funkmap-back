using System;
using System.Threading.Tasks;
using Funkmap.Common.Models;
using Funkmap.Cqrs.Abstract;
using Funkmap.Messenger.Command.Commands;
using Funkmap.Messenger.Hubs.Abstract;
using Funkmap.Messenger.Services.Abstract;
using Microsoft.AspNetCore.SignalR;

namespace Funkmap.Messenger.Hubs
{
//    [HubName("messenger")]
    public class MessengerHub : Hub<IMessengerHub>
    {
        private readonly IMessengerConnectionService _connectionService;
        private readonly ICommandBus _commandBus;

        public MessengerHub(IMessengerConnectionService connectionService,
                            ICommandBus commandBus)
        {
            _connectionService = connectionService;
            _commandBus = commandBus;
        }

        [HubMethodName("open-dialog")]
        public BaseResponse SetOpenedDialog(string dialogId)
        {
            var connectionId = Context.ConnectionId;
            var isSucces = _connectionService.SetOpenedDialog(connectionId, dialogId);
            
            var login = (string)Context.Items["login"];

            //у пользователя нет открытых диалогов
            if (String.IsNullOrEmpty(dialogId))
            {
                return new BaseResponse {Success = true};
            }

            _commandBus.ExecuteAsync(new ReadMessagesCommand
            {
                DialogId = dialogId,
                UserLogin = login,
                ReadTime = DateTime.UtcNow
            });

            return new BaseResponse {Success = isSucces};
        }

        public override Task OnConnectedAsync()
        {
            var connectionId = Context.ConnectionId;
            var login = (string)Context.Items["login"];

            _connectionService.AddOnlineUser(connectionId, login);
            Clients.All.OnUserConnected(login);

            return base.OnConnectedAsync();
        }


        public override Task OnDisconnectedAsync(Exception ex)
        {
            var connectionId = Context.ConnectionId;
            _connectionService.RemoveOnlineUser(connectionId, out var login);
            Clients.All.OnUserDisconnected(login);

            return base.OnDisconnectedAsync(ex);
        }
    }
}
