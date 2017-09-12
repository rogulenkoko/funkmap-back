using System;

namespace Funkmap.Notifications.Contracts
{
    public abstract class Notification
    {
        public abstract NotificationType NotificationType { get; }

        public string RecieverLogin { get; set; }
    }

    public enum NotificationType
    {
        BandInvite = 1
    }
}
