using System;
using System.Threading.Tasks;
using Funkmap.Common.Data.Mongo;
using Funkmap.Statistics.Data.Entities;
using MongoDB.Driver;

namespace Funkmap.Statistics.Data.Repositories.Abstract
{
    public class StatisticsMongoRepository<T> : MongoRepository<T> where T : BaseStatisticsEntity
    {
        public StatisticsMongoRepository(IMongoCollection<T> collection) : base(collection)
        {
        }

        public override async Task UpdateAsync(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            entity.LastUpdate = DateTime.UtcNow;
            var result = await _collection.FindOneAndReplaceAsync(x => x.Id == entity.Id, entity);
            if (result == null) await CreateAsync(entity);
        }

        
    }
}
