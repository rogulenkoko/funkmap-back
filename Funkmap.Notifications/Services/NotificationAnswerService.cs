using System;
using Funkmap.Common.Redis.Abstract;
using Funkmap.Common.Redis.Options;
using Funkmap.Notifications.Contracts;
using Funkmap.Notifications.Services.Abstract;

namespace Funkmap.Notifications.Services
{
    public class NotificationAnswerService : INotificationAnswerService
    {
        private readonly IMessageQueue _messageQueue;

        public NotificationAnswerService(IMessageQueue messageQueue)
        {
            _messageQueue = messageQueue;
        }

        public void PublishNotificationAnswer(NotificationAnswer answer)
        {
            if(answer?.Notification == null) throw new ArgumentNullException();
            var options = new MessageQueueOptions()
            {
                SpecificKey = answer.Notification.Type,
                SerializerOptions = new SerializerOptions() {HasAbstractMember = true}
            };
            _messageQueue.PublishAsync(answer, options);
        }
    }
}
