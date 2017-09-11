
namespace Funkmap.Notifications.Contracts
{
    public abstract class NotificationBack
    {
        /// <summary>
        /// Идентификатор изначального реквеста
        /// </summary>
        public string RequestId { get; set; }

        public bool? Answer { get; set; }
    }
}
