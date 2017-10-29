using System;
using Funkmap.Common.Redis.Abstract;
using Funkmap.Common.Redis.Options;
using Funkmap.Notifications.Contracts;
using Funkmap.Notifications.Contracts.Abstract;
using Funkmap.Notifications.Contracts.Specific;
using Funkmap.Notifications.Data.Abstract;
using Funkmap.Notifications.Data.Entities;
using Funkmap.Notifications.Services.Abstract;

namespace Funkmap.Notifications.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IMessageQueue _messageQueue;
        private readonly INotificationRepository _notificationRepository;

        public NotificationService(IMessageQueue messageQueue, INotificationRepository notificationRepository)
        {
            _messageQueue = messageQueue;
            _notificationRepository = notificationRepository;
        }

        public void InitHandlers()
        {
            _messageQueue.Subscribe<BandInviteNotification>(Handle);
        }

        private void Handle(NotificationBase notification)
        {
            var notificatinEntity = new NotificationEntity()
            {
                Date = DateTime.UtcNow,
                InnerNotification = notification,
                IsRead = false,
                RecieverLogin = notification.RecieverLogin,
                NotificationType = notification.Type,
                SenderLogin = notification.SenderLogin
            };

            //уведомление по сигнал р

            _notificationRepository.CreateAsync(notificatinEntity);
        }

        public void PublishNotificationAnswer(NotificationAnswer answer)
        {
            if (answer?.Notification == null) throw new ArgumentNullException();
            var options = new MessageQueueOptions()
            {
                SpecificKey = answer.Notification.Type,
                SerializerOptions = new SerializerOptions() { HasAbstractMember = true }
            };
            _messageQueue.PublishAsync(answer, options);
        }
    }
}
