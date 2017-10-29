namespace Funkmap.Notifications.Contracts.Abstract
{
    public abstract class NotificationBase
    {
        public abstract NotificationType Type { get; }
        public string SenderLogin { get; set; }
        public string RecieverLogin { get; set; }
    }
}
