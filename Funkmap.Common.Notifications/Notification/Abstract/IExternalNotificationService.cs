using System.Threading.Tasks;

namespace Funkmap.Common.Notifications.Notification.Abstract
{
    public interface IExternalNotificationService
    {
        Task<bool> TrySendNotificationAsync(Notification notification, NotificationOptions options = null);
    }
}
