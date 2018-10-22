using System.Threading.Tasks;
using Funkmap.Cqrs.Abstract;
using Funkmap.Notifications.Domain.Events;
using Funkmap.Notifications.Domain.Services.Abstract;
using Funkmap.Notifications.Hubs;
using Microsoft.AspNet.SignalR;

namespace Funkmap.Notifications.SignalR
{
    public class NotificationsSignalRHandler : IEventHandler<NotificationSavedEvent>
    {
        private readonly IEventBus _messageBus;
        private readonly INotificationsConnectionService _connectionService;

        public NotificationsSignalRHandler(IEventBus messageBus, INotificationsConnectionService connectionService)
        {
            _messageBus = messageBus;
            _connectionService = connectionService;
        }

        public void InitHandlers()
        {
            _messageBus.Subscribe<NotificationSavedEvent>(Handle);
        }

        public async Task Handle(NotificationSavedEvent @event)
        {
            //уведомление по SignalR
            var recievers = _connectionService.GetConnectionIdsByLogin(@event.Notification.ReceiverLogin);

            await GlobalHost.ConnectionManager.GetHubContext<NotificationsHub, INotificationsHub>()
                .Clients.Clients(recievers)
                .OnNotificationRecieved(@event.Notification);
        }

        
    }
}
