using Funkmap.Feedback.Entities;

namespace Funkmap.Feedback.Events
{
    public class FeedbackSavedEvent
    {
        public FeedbackSavedEvent(FeedbackEntity feedback)
        {
            Feedback = feedback;
        }

        public FeedbackEntity Feedback { get; }
        
    }
}
