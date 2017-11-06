using System;
using System.Linq;
using System.Threading.Tasks;
using Funkmap.Common;
using Funkmap.Common.Data.Mongo;
using Funkmap.Data.Entities.Abstract;
using Funkmap.Statistics.Data.Entities;
using Funkmap.Statistics.Data.Repositories.Abstract;
using MongoDB.Driver;

namespace Funkmap.Statistics.Data.Repositories
{
    public class EntityTypeStatisticsRepository : MongoRepository<EntityTypeStatisticsEntity>, IStatisticsRepository
    {
        private readonly IMongoCollection<BaseEntity> _profileCollection;
        public EntityTypeStatisticsRepository(IMongoCollection<EntityTypeStatisticsEntity> collection,
                                               IMongoCollection<BaseEntity> profileCollection) : base(collection)
        {
            _profileCollection = profileCollection;
        }

        public async Task<BaseStatisticsEntity> BuildFullStatisticsAsync()
        {
            //db.bases.aggregate({$group: { _id: "$t", count: {$sum: 1} } })

            var statistics = await _profileCollection.Aggregate()
                .Group(x => x.EntityType, entities => new CountStatisticsEntity<EntityType>()
                    {
                        Key = entities.Key,
                        Count = entities.Count()
                    }
                ).ToListAsync();
            var statistic = new EntityTypeStatisticsEntity()
            {
                CountStatistics = statistics
            };
            return statistic;

        }

        public async Task<BaseStatisticsEntity> BuildStatisticsAsync(DateTime begin, DateTime end)
        {
            var filter = Builders<BaseEntity>.Filter.Gte(x => x.CreationDate, begin) &
                         Builders<BaseEntity>.Filter.Lte(x => x.CreationDate, end);

            var statistics = await _profileCollection.Aggregate()
                .Match(filter)
                .Group(x => x.EntityType, entities => new CountStatisticsEntity<EntityType>()
                    {
                        Key = entities.Key,
                        Count = entities.Count()
                    }
                ).ToListAsync();
            var statistic = new EntityTypeStatisticsEntity()
            {
                CountStatistics = statistics
            };
            return statistic;
        }

       

        public override async Task UpdateAsync(EntityTypeStatisticsEntity entity)
        {
            if(entity == null) throw new ArgumentNullException(nameof(entity));
            entity.LastUpdate = DateTime.UtcNow;
            var result = await _collection.FindOneAndReplaceAsync(x => x.Id == entity.Id, entity);
            if (result == null) await CreateAsync(entity);
        }

        public StatisticsType StatisticsType => StatisticsType.EntityType;
    }
}
