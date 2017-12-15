using Funkmap.Notifications.Contracts.Abstract;

namespace Funkmap.Notifications.Contracts.Specific.BandInvite
{
    public class BandInviteConfirmationNotification : NotificationBase
    {
        public override NotificationType Type => NotificationType.BandInviteConfirmation;
        public override bool NeedAnswer => false;

        public string BandLogin { get; set; }
        public string BandName { get; set; }
        public string InvitedMusicianLogin { get; set; }
        public bool Answer { get; set; }
    }
}
