using System.Collections.Generic;
using System.Threading.Tasks;
using Funkmap.Common.Data.Mongo;
using Funkmap.Notifications.Contracts;
using Funkmap.Notifications.Data.Abstract;
using Funkmap.Notifications.Data.Entities;
using MongoDB.Driver;

namespace Funkmap.Notifications.Data
{
    public class NotificationRepository : MongoRepository<NotificationEntity>, INotificationRepository
    {
        public NotificationRepository(IMongoCollection<NotificationEntity> collection) : base(collection)
        {
        }

        public override Task UpdateAsync(NotificationEntity entity)
        {
            throw new System.NotImplementedException();
        }

        public async Task<List<NotificationEntity>> GetUserNotificationsAsync(string login)
        {
            var filter = Builders<NotificationEntity>.Filter.Eq(x => x.RecieverLogin, login);
            var sort = Builders<NotificationEntity>.Sort.Descending(x => x.Date);
            var notifications = await _collection.Find(filter).Sort(sort).ToListAsync();

            var update = Builders<NotificationEntity>.Update.Set(x => x.IsRead, true);
            _collection.UpdateManyAsync(filter, update);


            return notifications;
        }

        public async Task<long> GetNewNotificationsCountAsync(string login)
        {
            var filter = Builders<NotificationEntity>.Filter.Eq(x => x.RecieverLogin, login) & Builders<NotificationEntity>.Filter.Eq(x => x.IsRead, false);
            var count = await _collection.Find(filter).CountAsync();
            return count;
        }
    }
}
