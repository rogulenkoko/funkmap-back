using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Funkmap.Common;
using Funkmap.Common.RedisMq;
using Funkmap.Notifications.Contracts;
using Funkmap.Notifications.Data.Abstract;
using Funkmap.Notifications.Data.Entities;
using Funkmap.Notifications.Hubs;
using Funkmap.Notifications.Mappers;
using Funkmap.Notifications.Models;
using Funkmap.Notifications.Services.Abstract;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Infrastructure;
using Newtonsoft.Json;
using ServiceStack.Messaging;

namespace Funkmap.Notifications.Services
{
    public class NotificationsService<TRequest, TResponse> : RedisMqProducer, IRedisMqConsumer, INotificationsService where TRequest : Notification
                                                                                                                      where TResponse : NotificationBack
    {
        private readonly IMessageService _messageService;
        private readonly INotificationRepository _notificationRepository;
        private readonly INotificationsConnectionService _connectionService;
        public NotificationsService(IMessageFactory redisMqFactory, 
                                    IMessageService messageService,
                                    INotificationRepository notificationRepository,
                                    INotificationsConnectionService connectionService,
                                    NotificationType notificationType) : base(redisMqFactory)
        {
            _messageService = messageService;
            _notificationRepository = notificationRepository;
            _connectionService = connectionService;
            NotificationType = notificationType;
        }

        public NotificationType NotificationType { get; }

        public void InitHandlers()
        {
            _messageService.RegisterHandler<TRequest>(request => HandleRequest(request?.GetBody()));
        }

        private async Task<bool> HandleRequest(TRequest request)
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
            await _notificationRepository.CreateAsync(notificatinEntity);

            //IHubContext hubContext = GlobalHost.DependencyResolver.Resolve<IConnectionManager>().GetHubContext<NotificationsHub>();
            //var user = new List<string>() {request.RecieverLogin};
            //var connections = _connectionService.GetConnectionIdsByLogins(user).ToArray();
            //var notification = notificatinEntity.ToNotificationModel();
            //hubContext.Clients.All.onNotificationRecieved(notification);

            return true;
        }

        public void PublishBackRequest(NotificationBack request)
        {
            Publish<TResponse>(request.ToSpecificNotificationBack(NotificationType) as TResponse);
        }
    }
}
