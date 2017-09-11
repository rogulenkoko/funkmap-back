using System;
using System.Collections.Generic;
using Funkmap.Common.RedisMq;
using Funkmap.Notifications.Contracts;
using Funkmap.Notifications.Contracts.Funkmap;
using Funkmap.Notifications.Services.Abstract;
using ServiceStack.Messaging;

namespace Funkmap.Notifications.Services
{
    public class BandInviteNotificationsService : RedisMqProducer, IRedisMqConsumer, IBackNotificationService
    {
        private readonly IMessageService _messageService;
        public BandInviteNotificationsService(IMessageFactory redisMqFactory, IMessageService messageService) : base(redisMqFactory)
        {
            _messageService = messageService;
        }

        public void InitHandlers()
        {
            _messageService.RegisterHandler<InviteToGroupRequest>(request => HandleInviteToGroup(request?.GetBody()));
        }

        private bool HandleInviteToGroup(InviteToGroupRequest request)
        {
            //todo запись в базу, реагирование через хаб
            return true;
        }

        public NotificationType NotificationType => NotificationType.BandInvite;

        public ICollection<Notification> GetNotifications(string login)
        {
            throw new NotImplementedException();
        }

        public void PublishBackRequest(NotificationBackRequest request)
        {
            var back = new InviteToGroupBackRequest
            {
                Answer = request.Answer,
                RequestId = request.RequestId
            };
            Publish<InviteToGroupBackRequest>(back);
        }
    }
}
