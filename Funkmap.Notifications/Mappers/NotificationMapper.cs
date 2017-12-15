using Funkmap.Notifications.Data.Entities;
using Funkmap.Notifications.Models;

namespace Funkmap.Notifications.Mappers
{
    public static class NotificationMapper
    {
        public static NotificationModel ToNotificationModel(this NotificationEntity source)
        {
            if (source == null) return null;
            return new NotificationModel()
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
    }
}

