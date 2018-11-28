using System.Threading.Tasks;
using Funkmap.Notifications.Contracts.Models;

namespace Funkmap.Notifications.Contracts.Abstract
{
    /// <summary>
    /// Service for Funkmap notifications (custom, email)
    /// </summary>
    public interface IFunkmapNotificationService
    {
        Task NotifyAsync<TNotification>(TNotification notification, string receiver, string sender) where TNotification : class;
        
        Task EmailNotifyAsync<TNotification>(TNotification notification, string receiver, string sender = null) where TNotification : IFunkmapEmailNotification;

        Task AnswerAsync(NotificationAnswer answer);
    }
}
