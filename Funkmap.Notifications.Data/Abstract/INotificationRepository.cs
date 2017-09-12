using System.Collections.Generic;
using System.Threading.Tasks;
using Funkmap.Common.Data.Mongo.Abstract;
using Funkmap.Notifications.Contracts;
using Funkmap.Notifications.Data.Entities;

namespace Funkmap.Notifications.Data.Abstract
{
    public interface INotificationRepository : IMongoRepository<NotificationEntity>
    {
        Task<ICollection<NotificationEntity>> GetUserNotificationsAsync(string login);
    }
}
