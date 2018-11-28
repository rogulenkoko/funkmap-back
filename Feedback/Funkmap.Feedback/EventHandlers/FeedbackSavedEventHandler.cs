using System.Configuration;
using System.Threading.Tasks;
using Funkmap.Cqrs.Abstract;
using Funkmap.Feedback.Command;
using Funkmap.Feedback.Models;
using Funkmap.Notifications.Contracts.Abstract;

namespace Funkmap.Feedback.EventHandlers
{
    /// <summary>
    /// FeedbackSavedEvent handler
    /// </summary>
    public class FeedbackSavedEventHandler : IEventHandler<FeedbackSavedEvent>
    {
        private readonly IEventBus _eventBus;
        private readonly IFunkmapNotificationService _notificationService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="eventBus"><see cref="IEventBus"/></param>
        /// <param name="notificationService"><see cref="IFunkmapNotificationService"/></param>
        public FeedbackSavedEventHandler(IEventBus eventBus, IFunkmapNotificationService notificationService)
        {
            _eventBus = eventBus;
            _notificationService = notificationService;
        }

        ///<inheritdoc cref="IEventHandler.InitHandlers"/>
        public void InitHandlers()
        {
            _eventBus.Subscribe<FeedbackSavedEvent>(Handle);
        }

        ///<inheritdoc cref="IEventHandler.Handle"/>
        public async Task Handle(FeedbackSavedEvent @event)
        {
            var receiver = ConfigurationManager.AppSettings["email"];

            var notification = new FeedbackEmailNotification(@event.Feedback);
            await _notificationService.EmailNotifyAsync(notification, receiver);
        }
    }
}
