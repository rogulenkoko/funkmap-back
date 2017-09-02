namespace Funkmap.Common.Notifications.Notification
{
    public abstract class Notification
    {
        public string Receiver { get; set; }
        public virtual string Body { get; set; }
        public virtual string Subject { get; set; }
    }
}
