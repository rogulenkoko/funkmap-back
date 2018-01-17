using System.Configuration;
using Funkmap.Common.Cqrs.Abstract;
using Funkmap.Common.Notifications.Notification.Abstract;
using Funkmap.Feedback.Events;
using Funkmap.Feedback.Notifications;

namespace Funkmap.Feedback.EventHandlers
{
    public class FeedbackSavedEventHandler : IEventHandler<FeedbackSavedEvent>
    {
        private readonly IEventBus _eventBus;
        private readonly IExternalNotificationService _notificationService;

        public FeedbackSavedEventHandler(IEventBus eventBus, IExternalNotificationService notificationService)
        {
            _eventBus = eventBus;
            _notificationService = notificationService;
        }

        public void InitHandlers()
        {
            _eventBus.Subscribe<FeedbackSavedEvent>(Handle);
        }

        public void Handle(FeedbackSavedEvent @event)
        {
            var reciever = ConfigurationManager.AppSettings["email"];

            var message = new FeedbackNotification(reciever, @event.Feedback.FeedbackType, @event.Feedback.Message);

            _notificationService.TrySendNotificationAsync(message).GetAwaiter().GetResult();
        }
        
    }
}
