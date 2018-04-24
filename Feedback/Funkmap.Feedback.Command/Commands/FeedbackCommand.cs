using System.Collections.Generic;
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

        public ICollection<FeedbackContent> Content { get; set; }
    }

    public class FeedbackContent
    {
        public string Name { get; set; }
        public byte[] Data { get; set; }
    }
}
