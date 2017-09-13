using System;
using Funkmap.Notifications.Contracts;

namespace Funkmap.Notifications.Models
{
    public class NotificationModel
    {
        public string Id { get; set; }
        public NotificationType NotificationType { get; set; }
        public DateTime Date { get; set; }
        public bool IsRead { get; set; }
        public string RecieverLogin { get; set; }
        public string SenderLogin { get; set; }
        public Notification InnerNotification { get; set; }
    }
}
