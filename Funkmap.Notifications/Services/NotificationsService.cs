using System;
using System.Collections.Generic;
using Funkmap.Common.RedisMq;
using Funkmap.Notifications.Contracts;
using Funkmap.Notifications.Services.Abstract;
using ServiceStack.Messaging;

namespace Funkmap.Notifications.Services
{
    public class NotificationsService<TRequest, TResponse> : RedisMqProducer, IRedisMqConsumer, INotificationsService where TRequest : Notification
                                                                                                                      where TResponse : NotificationBack
    {
        private readonly IMessageService _messageService;
        public NotificationsService(IMessageFactory redisMqFactory, IMessageService messageService) : base(redisMqFactory)
        {
            _messageService = messageService;
        }

        public NotificationType NotificationType => NotificationType.BandInvite;

        public void InitHandlers()
        {
            _messageService.RegisterHandler<TRequest>(request => HandleRequest(request?.GetBody()));
        }

        private bool HandleRequest(TRequest request)
        {
            //todo запись в базу, реагирование через хаб
            return true;
        }
        
        public ICollection<Notification> GetNotifications(string login)
        {
            throw new NotImplementedException();
        }

        public void PublishBackRequest(NotificationBack request)
        {
            Publish<TResponse>(request as TResponse);
        }
    }
}
