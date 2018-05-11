using Funkmap.Cqrs.Abstract;
using Funkmap.Notifications.Contracts;
using Funkmap.Notifications.Contracts.Events;

namespace Funkmap.Notifications.Domain.Services.Abstract
{
    public interface INotificationService : IEventHandler<NotificationRecievedEvent>
    {
        void PublishNotificationAnswer(NotificationAnswer answer);
    }
}
