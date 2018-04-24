using Funkmap.Notifications.Contracts.Abstract;

namespace Funkmap.Notifications.Contracts
{
    public class NotificationAnswer
    {
        public bool Answer { get; set; }
        public NotificationBase Notification { get; set; }
    }
}
