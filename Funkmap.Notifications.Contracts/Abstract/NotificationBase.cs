using Funkmap.Notifications.Contracts.Specific.BandInvite;
using MongoDB.Bson.Serialization.Attributes;

namespace Funkmap.Notifications.Contracts.Abstract
{
    //todo как-то разрулить эти наследования
    [BsonKnownTypes(typeof(BandInviteNotification), typeof(BandInviteConfirmationNotification))]
    public abstract class NotificationBase
    {
        public abstract NotificationType Type { get; }
        public abstract bool NeedAnswer { get; }

        public string SenderLogin { get; set; }
        public string RecieverLogin { get; set; }
    }
}
