using System.Threading.Tasks;

namespace Funkmap.Common.Notification.Abstract
{
    public interface INotificationService
    {
        Task<bool> SendNotification(Notification notification);
    }
}
