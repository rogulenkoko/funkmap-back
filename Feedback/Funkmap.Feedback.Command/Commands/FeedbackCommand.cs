using Funkmap.Feedback.Domain;

namespace Funkmap.Feedback.Command.Commands
{
    public class FeedbackCommand
    {
        public FeedbackCommand(FeedbackItem item)
        {
            Item = item;
        }

        public FeedbackItem Item { get; }
    }
}
