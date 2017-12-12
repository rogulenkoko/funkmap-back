using System.Threading.Tasks;

namespace Funkmap.Common.Notifications.Notification.Abstract
{
    public interface IExternalNotificationService
    {
        Task<bool> SendNotification(Notification notification);
    }
}
