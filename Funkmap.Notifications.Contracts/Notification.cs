
namespace Funkmap.Notifications.Contracts
{
    public abstract class Notification
    {
        public abstract NotificationType NotificationType { get; }

        public string RecieverLogin { get; set; }

        public string SenderLogin { get; set; }
    }

    public enum NotificationType
    {
        BandInvite = 1
    }
}
