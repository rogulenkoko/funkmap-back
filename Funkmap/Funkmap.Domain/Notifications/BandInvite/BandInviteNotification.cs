using Funkmap.Notifications.Contracts;

namespace Funkmap.Domain.Notifications.BandInvite
{
    [FunkmapNotification("band_invite", true)]
    public class BandInviteNotification
    {
        public string BandLogin { get; set; }
        public string BandName { get; set; }
        public string InvitedMusicianLogin { get; set; }
        
    }
}
