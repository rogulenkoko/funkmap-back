using Funkmap.Common.Notifications.Notification;
using Funkmap.Feedback.Entities;

namespace Funkmap.Feedback.Notifications
{
    public class FeedbackNotification : Notification
    {
        public FeedbackNotification(string receiver, FeedbackType feedbackType, string message) : base(receiver)
        {
            Subject = feedbackType.ToString("G");
            MainContent = message;
        }
    }
}
