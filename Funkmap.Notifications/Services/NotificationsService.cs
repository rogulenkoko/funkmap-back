using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Funkmap.Common.RedisMq;
using Funkmap.Notifications.Contracts;
using Funkmap.Notifications.Data.Abstract;
using Funkmap.Notifications.Data.Entities;
using Funkmap.Notifications.Mappers;
using Funkmap.Notifications.Models;
using Funkmap.Notifications.Services.Abstract;
using Newtonsoft.Json;
using ServiceStack.Messaging;

namespace Funkmap.Notifications.Services
{
    public class NotificationsService<TRequest, TResponse> : RedisMqProducer, IRedisMqConsumer, INotificationsService where TRequest : Notification
                                                                                                                      where TResponse : NotificationBack
    {
        private readonly IMessageService _messageService;
        private readonly INotificationRepository _notificationRepository;
        public NotificationsService(IMessageFactory redisMqFactory, 
                                    IMessageService messageService,
                                    INotificationRepository notificationRepository) : base(redisMqFactory)
        {
            _messageService = messageService;
            _notificationRepository = notificationRepository;
        }

        public NotificationType NotificationType => NotificationType.BandInvite;

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
                RecieverLogin = request.RecieverLogin
            };
            await _notificationRepository.CreateAsync(notificatinEntity);
            //todo запись в базу, реагирование через хаб
            return true;
        }
        
        //public async Task<ICollection<NotificationModel>> GetNotifications(string login)
        //{
        //    var entities = await _notificationRepository.GetUserNotificationsAsync(login);
        //    return entities.Select(x => x.ToNotification()).ToList();
        //}

        public void PublishBackRequest(NotificationBack request)
        {
            Publish<TResponse>(request as TResponse);
        }
    }
}
