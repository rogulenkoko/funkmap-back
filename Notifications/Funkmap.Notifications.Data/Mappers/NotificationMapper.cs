using System.Collections.Generic;
using System.Linq;
using Funkmap.Notifications.Data.Entities;
using Funkmap.Notifications.Domain.Models;
using Newtonsoft.Json;

namespace Funkmap.Notifications.Data.Mappers
{
    public static class NotificationMapper
    {
        public static List<Notification> ToNotifications(this IReadOnlyCollection<NotificationEntity> source)
        {
            return source?.Select(x => x.ToNotification()).ToList();
        }

        public static Notification ToNotification(this NotificationEntity source)
        {
            if (source == null) return null;
            return new Notification
            {
                Id = source.Id.ToString(),
                NotificationType = source.NotificationType,
                ReceiverLogin = source.ReceiverLogin,
                CreatedAt = source.CreatedAt,
                IsRead = source.IsRead,
                InnerNotificationJson = source.InnerNotificationJson,
                InnerNotification = JsonConvert.DeserializeObject<dynamic>(source.InnerNotificationJson),
                SenderLogin = source.SenderLogin,
                NeedAnswer = source.NeedAnswer
            };
        }

        public static NotificationEntity ToEntity(this Notification source)
        {
            if (source == null) return null;
            return new NotificationEntity
            {
                CreatedAt = source.CreatedAt,
                InnerNotificationJson = source.InnerNotificationJson,
                IsRead = false,
                NeedAnswer = source.NeedAnswer,
                NotificationType = source.NotificationType,
                ReceiverLogin = source.ReceiverLogin,
                SenderLogin = source.SenderLogin,
            };
        }
    }
}

