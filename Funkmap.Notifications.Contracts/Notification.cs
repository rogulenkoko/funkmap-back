using System;

namespace Funkmap.Notifications.Contracts
{
    public abstract class Notification
    {
        protected Notification()
        {
            Id = Guid.NewGuid().ToString();
        }

        public abstract NotificationType NotificationType { get; }

        public string Id { get; set; }
        public string Type { get; set; }
    }

    public enum NotificationType
    {
        BandInvite = 1
    }
}
