using Funkmap.Common.Redis.Abstract;
using Funkmap.Notifications.Contracts;

namespace Funkmap.Notifications.Services.Abstract
{
    public interface INotificationsService : IMessageHandler
    {
        NotificationType NotificationType { get; }

        void PublishBackRequest(NotificationBack request);
    }

   
}
