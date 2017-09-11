
using Funkmap.Notifications.Contracts;

namespace Funkmap.Notifications.Services.Abstract
{
    public interface IBackNotificationService : INotificationsService
    {
        void PublishBackRequest(NotificationBackRequest request);
    }
}
