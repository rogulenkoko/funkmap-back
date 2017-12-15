using System;
using System.Linq;
using Funkmap.Common.Redis.Abstract;
using Funkmap.Common.Redis.Options;
using Funkmap.Notifications.Contracts;
using Funkmap.Notifications.Contracts.Abstract;
using Funkmap.Notifications.Contracts.Specific;
using Funkmap.Notifications.Contracts.Specific.BandInvite;
using Funkmap.Notifications.Data.Abstract;
using Funkmap.Notifications.Data.Entities;
using Funkmap.Notifications.Hubs;
using Funkmap.Notifications.Mappers;
using Funkmap.Notifications.Services.Abstract;
using Microsoft.AspNet.SignalR;

namespace Funkmap.Notifications.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IMessageQueue _messageQueue;
        private readonly INotificationRepository _notificationRepository;
        private readonly INotificationsConnectionService _connectionService;

        public NotificationService(IMessageQueue messageQueue, INotificationRepository notificationRepository, INotificationsConnectionService connectionService)
        {
            _messageQueue = messageQueue;
            _notificationRepository = notificationRepository;
            _connectionService = connectionService;
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

            //уведомление по SignalR
            var recievers = _connectionService.GetConnectionIdsByLogin(notificatinEntity.RecieverLogin);

            GlobalHost.ConnectionManager.GetHubContext<NotificationsHub, INotificationsHub>()
                .Clients.Clients(recievers.ToList())
                .OnNotificationRecieved(notificatinEntity.ToNotificationModel());

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
