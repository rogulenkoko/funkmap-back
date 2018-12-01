using System.Threading.Tasks;
using Funkmap.Cqrs.Abstract;
using Funkmap.Notifications.Domain.Events;
using Funkmap.Notifications.Domain.Services.Abstract;
using Funkmap.Notifications.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Funkmap.Notifications.SignalR
{
    public class NotificationsSignalRHandler : IEventHandler<NotificationSavedEvent>
    {
        private readonly IEventBus _messageBus;
        private readonly INotificationsConnectionService _connectionService;
        private readonly IHubContext<NotificationsHub> _hubContext;

        public NotificationsSignalRHandler(IEventBus messageBus, 
            INotificationsConnectionService connectionService,
            IHubContext<NotificationsHub> hubContext)
        {
            _messageBus = messageBus;
            _connectionService = connectionService;
            _hubContext = hubContext;
        }

        public void InitHandlers()
        {
            _messageBus.Subscribe<NotificationSavedEvent>(Handle);
        }

        public async Task Handle(NotificationSavedEvent @event)
        {
            //уведомление по SignalR
            var receivers = _connectionService.GetConnectionIdsByLogin(@event.Notification.ReceiverLogin);

            await _hubContext.Clients.Clients(receivers)
                .SendAsync("OnNotificationRecieved", @event.Notification);
        }

        
    }
}
