using System.Configuration;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Common.Cqrs.Abstract;
using Funkmap.Common.Notifications.Notification;
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

        public async Task Handle(FeedbackSavedEvent @event)
        {
            var reciever = ConfigurationManager.AppSettings["email"];


            var sb = new StringBuilder(@event.Feedback.Message);

            if (@event.Feedback.Content != null && @event.Feedback.Content.Count > 0)
            {
                sb.AppendLine();
                sb.AppendLine("Приложенные файлы:");
                foreach (var contentItem in @event.Feedback.Content)
                {
                    sb.AppendLine(contentItem.DataUrl);
                }
                
            }

            var message = new FeedbackNotification(reciever, @event.Feedback.FeedbackType, sb.ToString());

            await _notificationService.TrySendNotificationAsync(message, new NotificationOptions() {UseTemplate = false});
        }
        
    }
}
