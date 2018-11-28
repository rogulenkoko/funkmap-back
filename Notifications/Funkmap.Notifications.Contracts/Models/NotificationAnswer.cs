namespace Funkmap.Notifications.Contracts.Models
{
    public class NotificationAnswer
    {
        public string NotificationJson { get; set; }
        public string Sender { get; set; }
        public string Receiver { get; set; }
        public string NotificationType { get; set; }

        public bool Answer { get; set; }
    }
}
