using System.Collections.Generic;
using System.Threading.Tasks;
using Funkmap.Notifications.Data.Entities;
using Funkmap.Notifications.Data.Mappers;
using Funkmap.Notifications.Domain.Abstract;
using Funkmap.Notifications.Domain.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Funkmap.Notifications.Data
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly IMongoCollection<NotificationEntity> _collection;
        
        public NotificationRepository(IMongoCollection<NotificationEntity> collection)
        {
            _collection = collection;
        }

        public async Task<Notification> GetAsync(string id)
        {
            var entity = await _collection.Find(x => x.Id == new ObjectId(id)).SingleOrDefaultAsync();
            return entity.ToNotification();
        }

        public async Task<Notification> CreateAsync(Notification notification)
        {
            var entity = notification.ToEntity();
            await _collection.InsertOneAsync(entity);
            return entity.ToNotification();
        }

        public async Task<List<Notification>> GetUserNotificationsAsync(string login)
        {
            var filter = Builders<NotificationEntity>.Filter.Eq(x => x.ReceiverLogin, login);
            var sort = Builders<NotificationEntity>.Sort.Descending(x => x.CreatedAt);
            var notifications = await _collection.Find(filter).Sort(sort).ToListAsync();

            var updateFilter = filter & Builders<NotificationEntity>.Filter.Eq(x => x.NeedAnswer, false);
            var update = Builders<NotificationEntity>.Update.Set(x => x.IsRead, true);
            await _collection.UpdateManyAsync(updateFilter, update);

            return notifications.ToNotifications();
        }

        public async Task<long> GetNewNotificationsCountAsync(string login)
        {
            var filter = Builders<NotificationEntity>.Filter.Eq(x => x.ReceiverLogin, login) & Builders<NotificationEntity>.Filter.Eq(x => x.IsRead, false);
            var count = await _collection.Find(filter).CountAsync();
            return count;
        }
    }
}
