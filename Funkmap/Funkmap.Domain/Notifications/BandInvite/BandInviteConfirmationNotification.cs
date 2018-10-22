using Funkmap.Notifications.Contracts;

namespace Funkmap.Domain.Notifications.BandInvite
{
    [FunkmapNotification("band_invite_confirmation", false)]
    public class BandInviteConfirmationNotification
    {
        public string BandLogin { get; set; }
        public string BandName { get; set; }
        public string InvitedMusicianLogin { get; set; }
        public bool Answer { get; set; }
    }
}
