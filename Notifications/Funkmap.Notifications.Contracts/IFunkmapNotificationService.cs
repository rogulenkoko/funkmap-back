using System.Threading.Tasks;

namespace Funkmap.Notifications.Contracts
{
    public interface IFunkmapNotificationService
    {
        Task NotifyAsync<TNotification>(TNotification notification, string receiver, string sender) where TNotification : class;

        Task AnswerAsync(NotificationAnswer answer);
    }
}
