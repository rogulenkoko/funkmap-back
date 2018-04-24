using Funkmap.Notifications.Domain.Models;

namespace Funkmap.Notifications.Domain.Events
{
    public class NotificationSavedEvent
    {
        public Notification Notification { get; set; }
    }
}
