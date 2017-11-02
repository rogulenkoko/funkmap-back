using System;
using System.Linq;
using System.Threading.Tasks;
using Funkmap.Common;
using Funkmap.Common.Data.Mongo;
using Funkmap.Data.Entities.Abstract;
using Funkmap.Statistics.Data.Entities;
using MongoDB.Driver;

namespace Funkmap.Statistics.Data.Repositories
{
    public class EntityTypeStatisticsRepository : MongoRepository<EntityTypeStatisticsEntity>
    {
        private readonly IMongoCollection<BaseEntity> _profileCollection;
        public EntityTypeStatisticsRepository(IMongoCollection<EntityTypeStatisticsEntity> collection,
                                               IMongoCollection<BaseEntity> profileCollection) : base(collection)
        {
            _profileCollection = profileCollection;
        }

        public async Task<EntityTypeStatisticsEntity> BuildFullStatisticsAsync()
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

        public async Task<EntityTypeStatisticsEntity> BuildStatisticsAsync(DateTime begin, DateTime end)
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

        public async Task MergeStatistics()
        {
            var typeFilter = Builders<EntityTypeStatisticsEntity>.Filter.Eq(x=>x.StatisticsType, StatisticsType.EntityType);
            var saved = await _collection.Find(typeFilter).SingleOrDefaultAsync();

            var begin = saved.LastUpdate;
            var end = DateTime.UtcNow;
            var newStatistics = await BuildStatisticsAsync(begin, end);
            var newStatisticsDictionary = newStatistics.CountStatistics.ToDictionary(x => x.Key);
            foreach (var countStatistic in saved.CountStatistics)
            {
                if (newStatisticsDictionary.ContainsKey(countStatistic.Key))
                {
                    countStatistic.Count += newStatisticsDictionary[countStatistic.Key].Count;
                }
            }
            await UpdateAsync(saved);
        }

        public override async Task UpdateAsync(EntityTypeStatisticsEntity entity)
        {
            if(entity == null) throw new ArgumentNullException(nameof(entity));
            entity.LastUpdate = DateTime.UtcNow;
            var result = await _collection.FindOneAndReplaceAsync(x => x.Id == entity.Id, entity);
            if (result == null) await CreateAsync(entity);
        }
    }
}
