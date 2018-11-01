using System.Configuration;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Cqrs.Abstract;
using Funkmap.Feedback.Command;

namespace Funkmap.Feedback.EventHandlers
{
    public class FeedbackSavedEventHandler : IEventHandler<FeedbackSavedEvent>
    {
        private readonly IEventBus _eventBus;

        public FeedbackSavedEventHandler(IEventBus eventBus)
        {
            _eventBus = eventBus;
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

            //var message = new FeedbackNotification(reciever, @event.Feedback.FeedbackType, sb.ToString());

            //todo send all this to our email
        }
        
    }
}
