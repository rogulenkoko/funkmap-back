namespace Funkmap.Notifications.Contracts.Models
{
    public class Notification
    {
        public string NotificationJson { get; set; }
        public string Sender { get; set; }
        public string Receiver { get; set; }
        public string NotificationType { get; set; }
    }
}
