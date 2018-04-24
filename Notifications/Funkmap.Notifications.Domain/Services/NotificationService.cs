using System;
using System.Threading.Tasks;
using Funkmap.Common.Cqrs;
using Funkmap.Common.Cqrs.Abstract;
using Funkmap.Notifications.Contracts.Events;
using Funkmap.Notifications.Domain.Abstract;
using Funkmap.Notifications.Domain.Events;
using Funkmap.Notifications.Domain.Models;
using Funkmap.Notifications.Domain.Services.Abstract;
using NotificationAnswer = Funkmap.Notifications.Contracts.NotificationAnswer;

namespace Funkmap.Notifications.Domain.Services
{
    public class NotificationService : INotificationService
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
            _eventBus.Subscribe<NotificationRecievedEvent>(Handle);
        }

        public async Task Handle(NotificationRecievedEvent @event)
        {
            var notificationBase = @event.NotificationBase;

            var notification = new Notification
            {
                Date = DateTime.UtcNow,
                InnerNotification = notificationBase,
                IsRead = false,
                RecieverLogin = notificationBase.RecieverLogin,
                NotificationType = notificationBase.Type,
                SenderLogin = notificationBase.SenderLogin,
                NeedAnswer = notificationBase.NeedAnswer
            };

            var savedNotification = await _notificationRepository.CreateAsync(notification);

            await _eventBus.PublishAsync(new NotificationSavedEvent() { Notification = savedNotification });
        }

        public void PublishNotificationAnswer(NotificationAnswer answer)
        {
            if (answer?.Notification == null) throw new ArgumentNullException();
            var options = new MessageQueueOptions()
            {
                SpecificKey = answer.Notification.Type,
                SerializerOptions = new SerializerOptions() { HasAbstractMember = true }
            };
            _eventBus.PublishAsync(answer, options);
        }
    }
}
