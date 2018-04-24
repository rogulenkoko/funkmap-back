using Funkmap.Notifications.Contracts.Abstract;

namespace Funkmap.Notifications.Contracts.Events
{
    public class NotificationRecievedEvent
    {
        public NotificationBase NotificationBase { get; set; }
    }
}
