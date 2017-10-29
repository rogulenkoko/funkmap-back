using Funkmap.Notifications.Contracts;

namespace Funkmap.Notifications.Services.Abstract
{
    public interface INotificationAnswerService
    {
        void PublishNotificationAnswer(NotificationAnswer answer);
    }
}
