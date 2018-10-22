using System;
using Newtonsoft.Json;

namespace Funkmap.Notifications.Domain.Models
{
    public class Notification
    {
        public string Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsRead { get; set; }
        public string ReceiverLogin { get; set; }
        public string SenderLogin { get; set; }
        [JsonIgnore]
        public string InnerNotificationJson { get; set; }
        public dynamic InnerNotification { get; set; }
        public string NotificationType { get; set; }
        public bool NeedAnswer { get; set; }
    }
}
