using System.Collections.Generic;
using System.Threading.Tasks;
using Funkmap.Notifications.Domain.Models;

namespace Funkmap.Notifications.Domain.Abstract
{
    public interface INotificationRepository
    {
        Task<Notification> GetAsync(string id);
        Task<Notification> CreateAsync(Notification notification);

        Task<List<Notification>> GetUserNotificationsAsync(string login);
        Task<long> GetNewNotificationsCountAsync(string login);

    }
}
