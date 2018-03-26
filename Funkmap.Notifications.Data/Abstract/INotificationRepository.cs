using System.Collections.Generic;
using System.Threading.Tasks;
using Funkmap.Common.Data.Mongo.Abstract;
using Funkmap.Notifications.Data.Entities;

namespace Funkmap.Notifications.Data.Abstract
{
    public interface INotificationRepository : IRepository<NotificationEntity>
    {
        Task<List<NotificationEntity>> GetUserNotificationsAsync(string login);
        Task<long> GetNewNotificationsCountAsync(string login);

    }
}
