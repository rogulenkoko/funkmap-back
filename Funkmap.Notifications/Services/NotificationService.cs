using System;
using System.Linq;
using System.Threading.Tasks;
using Funkmap.Common.Cqrs;
using Funkmap.Common.Cqrs.Abstract;
using Funkmap.Notifications.Contracts;
using Funkmap.Notifications.Contracts.Abstract;
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
        private readonly IEventBus _eventBus;
        private readonly INotificationRepository _notificationRepository;
        private readonly INotificationsConnectionService _connectionService;

        public NotificationService(IEventBus eventBus, INotificationRepository notificationRepository, INotificationsConnectionService connectionService)
        {
            _eventBus = eventBus;
            _notificationRepository = notificationRepository;
            _connectionService = connectionService;
        }

        public void InitHandlers()
        {
            _eventBus.Subscribe<BandInviteNotification>(Handle);
            _eventBus.Subscribe<BandInviteConfirmationNotification>(Handle);
        }

        public async Task Handle(NotificationBase notification)
        {
            var notificatinEntity = new NotificationEntity()
            {
                Date = DateTime.UtcNow,
                InnerNotification = notification,
                IsRead = false,
                RecieverLogin = notification.RecieverLogin,
                NotificationType = notification.Type,
                SenderLogin = notification.SenderLogin,
                NeedAnswer = notification.NeedAnswer
            };

            //уведомление по SignalR
            var recievers = _connectionService.GetConnectionIdsByLogin(notificatinEntity.RecieverLogin);

            await GlobalHost.ConnectionManager.GetHubContext<NotificationsHub, INotificationsHub>()
                 .Clients.Clients(recievers.ToList())
                 .OnNotificationRecieved(notificatinEntity.ToNotificationModel());


            await _notificationRepository.CreateAsync(notificatinEntity);
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
