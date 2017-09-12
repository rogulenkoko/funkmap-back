
namespace Funkmap.Notifications.Contracts
{
    public abstract class NotificationBack
    {
        /// <summary>
        /// Изначальное уведомление
        /// </summary>
        public Notification Notification { get; set; }

        public bool? Answer { get; set; }
    }
}
