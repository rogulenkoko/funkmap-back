using System;
using System.Linq;
using System.Threading.Tasks;
using Funkmap.Data.Entities;
using Funkmap.Data.Entities.Entities.Abstract;
using Funkmap.Statistics.Data.Entities;
using Funkmap.Statistics.Data.Repositories.Abstract;
using MongoDB.Driver;

namespace Funkmap.Statistics.Data.Repositories
{
    public class EntityTypeStatisticsRepository : StatisticsMongoRepository<EntityTypeStatisticsEntity>, IProfileStatisticsRepository
    {
        private readonly IMongoCollection<BaseEntity> _profileCollection;

        public StatisticsType StatisticsType => StatisticsType.EntityType;

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


        
    }
}
