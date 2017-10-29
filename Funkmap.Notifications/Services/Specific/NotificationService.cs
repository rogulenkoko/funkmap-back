using System;
using Funkmap.Common.Redis.Abstract;
using Funkmap.Notifications.Contracts.Abstract;
using Funkmap.Notifications.Contracts.Specific;
using Funkmap.Notifications.Data.Abstract;
using Funkmap.Notifications.Data.Entities;

namespace Funkmap.Notifications.Services.Specific
{
    public class NotificationService : IMessageHandler
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
    }
}
