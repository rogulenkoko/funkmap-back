namespace Funkmap.Notifications.Contracts.Models
{
    public class EmailNotification
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        
        public string Sender { get; set; }
        public string Receiver { get; set; }
    }
}