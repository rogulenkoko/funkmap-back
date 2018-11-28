using Funkmap.Feedback.Domain;

namespace Funkmap.Feedback.Command
{
    public class FeedbackSavedEvent
    {
        public FeedbackSavedEvent(FeedbackItem feedback)
        {
            Feedback = feedback;
        }

        public FeedbackItem Feedback { get; }
        
    }
}
