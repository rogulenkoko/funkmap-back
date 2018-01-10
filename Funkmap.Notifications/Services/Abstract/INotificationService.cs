using Funkmap.Common.Cqrs.Abstract;
using Funkmap.Notifications.Contracts;
using Funkmap.Notifications.Contracts.Abstract;

namespace Funkmap.Notifications.Services.Abstract
{
    public interface INotificationService : IEventHandler<NotificationBase>
    {
        void PublishNotificationAnswer(NotificationAnswer answer);
    }
}
