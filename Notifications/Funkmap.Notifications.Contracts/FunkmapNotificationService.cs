using System;
using System.Reflection;
using System.Threading.Tasks;
using Funkmap.Cqrs;
using Funkmap.Cqrs.Abstract;
using Newtonsoft.Json;

namespace Funkmap.Notifications.Contracts
{
    public class FunkmapNotificationService : IFunkmapNotificationService
    {
        private readonly IEventBus _eventBus;

        public FunkmapNotificationService(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public async Task NotifyAsync<TNotification>(TNotification notification, string receiver, string sender) where TNotification : class 
        {
            var notificationName = typeof(TNotification).GetCustomAttribute<FunkmapNotificationAttribute>()?.Name;

            if (string.IsNullOrEmpty(notificationName))
            {
                throw new InvalidOperationException($"Notification has not {nameof(FunkmapNotificationAttribute)} or it's name is empty.");
            }

            var options = new MessageQueueOptions
            {
                SpecificKey = notificationName
            };

            var @event = new Notification
            {
                NotificationJson = JsonConvert.SerializeObject(notification),
                Receiver = receiver,
                Sender = sender,
                NotificationType = notificationName
            };

            await _eventBus.PublishAsync(@event, options);
        }

        public async Task AnswerAsync(NotificationAnswer answer)
        {
            if (string.IsNullOrEmpty(answer?.NotificationJson)) throw new ArgumentNullException();

            var options = new MessageQueueOptions
            {
                SpecificKey = answer.NotificationType,
            };
            await _eventBus.PublishAsync(answer, options);
        }
    }
}
