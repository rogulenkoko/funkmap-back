using System.Collections.Generic;
using System.Linq;
using Funkmap.Notifications.Data.Entities;
using Funkmap.Notifications.Domain.Models;

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
            return new Notification()
            {
                Id = source.Id.ToString(),
                NotificationType = source.NotificationType,
                RecieverLogin = source.RecieverLogin,
                Date = source.Date,
                IsRead = source.IsRead,
                InnerNotification = source.InnerNotification,
                SenderLogin = source.SenderLogin,
                NeedAnswer = source.NeedAnswer
            };
        }

        public static NotificationEntity ToEntity(this Notification source)
        {
            if (source == null) return null;
            return new NotificationEntity
            {
                Date = source.Date,
                InnerNotification = source.InnerNotification,
                IsRead = false,
                NeedAnswer = source.NeedAnswer,
                NotificationType = source.NotificationType,
                RecieverLogin = source.RecieverLogin,
                SenderLogin = source.SenderLogin
            };
        }
    }
}

