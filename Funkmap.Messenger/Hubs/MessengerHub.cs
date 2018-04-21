using System;
using System.Linq;
using System.Threading.Tasks;
using Funkmap.Common.Cqrs.Abstract;
using Funkmap.Common.Filters;
using Funkmap.Common.Models;
using Funkmap.Messenger.Command.Commands;
using Funkmap.Messenger.Hubs.Abstract;
using Funkmap.Messenger.Mappers;
using Funkmap.Messenger.Models;
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
            
            var login = Context.QueryString["login"];

            //у пользователя нет открытых диалогов
            if (String.IsNullOrEmpty(dialogId))
            {
                return new BaseResponse() {Success = true};
            }

            _commandBus.Execute(new ReadMessagesCommand
            {
                DialogId = dialogId,
                UserLogin = login,
                ReadTime = DateTime.UtcNow
            });

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
