using System.Threading.Tasks;
using Funkmap.Common.Filters;
using Funkmap.Notifications.Services.Abstract;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace Funkmap.Notifications.Hubs
{
    [HubName("notifications")]
    [ValidateRequestModel]
    public class NotificationsHub : Hub
    {
        private readonly INotificationsService _notificationsService;

        public NotificationsHub(INotificationsService notificationsService)
        {
            _notificationsService = notificationsService;
        }


        public override Task OnConnected()
        {
            var connectionId = Context.ConnectionId;
            var login = Context.QueryString["login"];


            return base.OnConnected();
        }


        public override Task OnDisconnected(bool stopCalled)
        {

            var connectionId = Context.ConnectionId;


            return base.OnDisconnected(stopCalled);
        }
    }
}
