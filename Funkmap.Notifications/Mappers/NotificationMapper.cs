using Funkmap.Contracts.Notifications;
using Funkmap.Notifications.Contracts;
using Funkmap.Notifications.Data.Entities;
using Funkmap.Notifications.Models;
using Newtonsoft.Json;

namespace Funkmap.Notifications.Mappers
{
    public static class NotificationMapper
    {
        public static NotificationModel ToNotification(this NotificationEntity source)
        {
            if (source == null) return null;

            var innerNotificationJson = source.InnerNotificationJson;
            Notification innerNotification;

            switch (source.NotificationType)
            {
                case NotificationType.BandInvite:
                    innerNotification = JsonConvert.DeserializeObject<InviteToBandRequest>(innerNotificationJson);
                    break;
                default:  innerNotification = null;
                    break;

            }
            return new NotificationModel()
            {
                NotificationType = source.NotificationType,
                RecieverLogin = source.RecieverLogin,
                Date = source.Date,
                IsRead = source.IsRead,
                InnerNotification = innerNotification
            };
        }
    }
}
