using System;
using System.Threading.Tasks;
using Funkmap.Cqrs.Abstract;
using Funkmap.Notifications.Domain.Abstract;
using Funkmap.Notifications.Domain.Events;
using Notification = Funkmap.Notifications.Contracts.Models.Notification;

namespace Funkmap.Notifications.Domain.Services
{
    public class NotificationService : IEventHandler<Notification>
    {
        private readonly IEventBus _eventBus;
        private readonly INotificationRepository _notificationRepository;

        public NotificationService(IEventBus eventBus, INotificationRepository notificationRepository)
        {
            _eventBus = eventBus;
            _notificationRepository = notificationRepository;
        }

        public void InitHandlers()
        {
            _eventBus.Subscribe<Notification>(Handle);
        }

        public async Task Handle(Notification @event)
        {
            var notification = new Models.Notification
            {
                CreatedAt = DateTime.UtcNow,
                InnerNotificationJson = @event.NotificationJson,
                IsRead = false,
                ReceiverLogin = @event.Receiver,
                SenderLogin = @event.Sender,
            };

            var savedNotification = await _notificationRepository.CreateAsync(notification);

            await _eventBus.PublishAsync(new NotificationSavedEvent { Notification = savedNotification });
        }
    }
}
