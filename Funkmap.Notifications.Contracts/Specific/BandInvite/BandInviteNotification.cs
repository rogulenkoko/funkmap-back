using Funkmap.Notifications.Contracts.Abstract;

namespace Funkmap.Notifications.Contracts.Specific.BandInvite
{
    public class BandInviteNotification : NotificationBase
    {
        public string BandLogin { get; set; }
        public string BandName { get; set; }
        public string InvitedMusicianLogin { get; set; }
        public override NotificationType Type => NotificationType.BandInvite;
    }
}
