using System.Threading.Tasks;
using Funkmap.Common.Filters;
using Funkmap.Notifications.Services.Abstract;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace Funkmap.Notifications.Hubs
{
    [HubName("notifications")]
    [ValidateRequestModel]
    public class NotificationsHub : Hub<INotificationsHub>
    {
        private readonly INotificationsConnectionService _connectionService;

        public NotificationsHub(INotificationsConnectionService connectionService)
        {
            _connectionService = connectionService;
        }
        
        public override Task OnConnected()
        {
            var connectionId = Context.ConnectionId;
            var login = Context.QueryString["login"];

            _connectionService.AddOnlineUser(connectionId, login);

            return base.OnConnected();
        }


        public override Task OnDisconnected(bool stopCalled)
        {

            var connectionId = Context.ConnectionId;

            string login;
            _connectionService.RemoveOnlineUser(connectionId, out login);

            return base.OnDisconnected(stopCalled);
        }
    }
}
