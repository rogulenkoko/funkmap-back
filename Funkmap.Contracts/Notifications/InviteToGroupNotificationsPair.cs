using System;
using Funkmap.Notifications.Contracts;

namespace Funkmap.Contracts.Notifications
{
    public class InviteToGroupNotificationsPair : INotificationTypesPair
    {
        public Type RequestType => typeof(InviteToBandRequest);
        public Type ResponseType => typeof(InviteToBandBack);
    }
    
    public class InviteToBandRequest : Notification
    {
        public string BandLogin { get; set; }
        public string BandName { get; set; }
        public string InvitedMusicianLogin { get; set; }
        public string InviterLogin { get; set; }
        public override NotificationType NotificationType => NotificationType.BandInvite;
    }

    public class InviteToBandBack : NotificationBack
    {
        
    }
}
