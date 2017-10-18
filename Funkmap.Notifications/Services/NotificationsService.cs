using System;
using Funkmap.Common.Redis.Abstract;
using Funkmap.Notifications.Contracts;
using Funkmap.Notifications.Data.Abstract;
using Funkmap.Notifications.Data.Entities;
using Funkmap.Notifications.Mappers;
using Funkmap.Notifications.Services.Abstract;
using Newtonsoft.Json;

namespace Funkmap.Notifications.Services
{
    public class NotificationsService<TRequest, TResponse> : INotificationsService where TRequest : Notification where TResponse : NotificationBack
    {
        private readonly IMessageQueue _messageQueue;
        private readonly INotificationRepository _notificationRepository;
        private readonly INotificationsConnectionService _connectionService;
        public NotificationsService(IMessageQueue messageQueue,
                                    INotificationRepository notificationRepository,
                                    INotificationsConnectionService connectionService,
                                    NotificationType notificationType)
        {
            _messageQueue = messageQueue;
            _notificationRepository = notificationRepository;
            _connectionService = connectionService;
            NotificationType = notificationType;
        }

        public NotificationType NotificationType { get; }

        public void InitHandlers()
        {
            _messageQueue.Subscribe<TRequest>(HandleRequest);
        }

        private void HandleRequest(TRequest request)
        {
            var notificatinEntity = new NotificationEntity()
            {
                Date = DateTime.UtcNow,
                InnerNotificationJson = JsonConvert.SerializeObject(request),
                IsRead = false,
                RecieverLogin = request.RecieverLogin,
                NotificationType = request.NotificationType,
                SenderLogin = request.SenderLogin
                
            };
            _notificationRepository.CreateAsync(notificatinEntity);

            //IHubContext hubContext = GlobalHost.DependencyResolver.Resolve<IConnectionManager>().GetHubContext<NotificationsHub>();
            //var user = new List<string>() {request.RecieverLogin};
            //var connections = _connectionService.GetConnectionIdsByLogins(user).ToArray();
            //var notification = notificatinEntity.ToNotificationModel();
            //hubContext.Clients.All.onNotificationRecieved(notification);
        }

        public void PublishBackRequest(NotificationBack request)
        {
            var backRequest = request.ToSpecificNotificationBack(NotificationType);
            _messageQueue.PublishAsync(backRequest);
        }
    }
}
