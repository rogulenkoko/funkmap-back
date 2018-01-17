using Funkmap.Feedback.Entities;

namespace Funkmap.Feedback.Command.Commands
{
    public class FeedbackCommand
    {
        public FeedbackCommand(FeedbackType feedbackType, string message)
        {
            FeedbackType = feedbackType;
            Message = message;
        }

        public FeedbackType FeedbackType { get; }

        public string Message { get; }
    }
}
