using System;
using Funkmap.Notifications.Contracts;
using Funkmap.Notifications.Contracts.Abstract;

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
        public bool NeedAnswer { get; set; }
        public NotificationBase InnerNotification { get; set; }
    }
}
