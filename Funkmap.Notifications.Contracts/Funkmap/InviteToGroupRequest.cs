
namespace Funkmap.Notifications.Contracts.Funkmap
{
    public class InviteToGroupRequest : Notification
    {
        public string GroupLogin { get; set; }
        public string InvitedMusicianLogin { get; set; }
        public string InviterLogin { get; set; }
        public override NotificationType NotificationType => NotificationType.BandInvite;
    }
}
