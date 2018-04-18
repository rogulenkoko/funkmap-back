using Funkmap.Notifications.Contracts.Specific.BandInvite;

namespace Funkmap.Models.Notifications
{
    public class BandInviteNotificationAnswer
    {
        public bool Answer { get; set; }

        public BandInviteNotification Notification { get; set; }
    }
}
