using Funkmap.Common.Redis.Abstract;
using Funkmap.Notifications.Contracts;

namespace Funkmap.Notifications.Services.Abstract
{
    public interface INotificationService : IMessageHandler
    {
        void PublishNotificationAnswer(NotificationAnswer answer);
    }
}
