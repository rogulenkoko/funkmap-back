using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Funkmap.Common;
using Funkmap.Common.Data.Mongo;
using Funkmap.Data.Entities.Abstract;
using Funkmap.Statistics.Data.Entities;
using Funkmap.Statistics.Data.Repositories.Abstract;
using MongoDB.Driver;

namespace Funkmap.Statistics.Data.Repositories
{
    public class TopEntityStatisticsRepository : MongoRepository<TopEntityStatisticsEntity>, IProfileStatisticsRepository
    {
        private readonly IMongoCollection<BaseEntity> _profileCollection;
        public async Task<BaseStatisticsEntity> BuildFullStatisticsAsync()
        {
            var statistics = await _profileCollection.Aggregate()
              
              .Group(x => x.EntityType, entities => new CountStatisticsEntity<EntityType>()
              {//todo нужно написать нозмальный запрос db.bases.aggregate({$group: { _id: "$t", count:{ $sum: "$fav".length-1} } })
                  Key = entities.Key,
                    Count = entities.GetEnumerator().Current.FavoriteFor.Count
                }).ToListAsync();
            var statistic = new TopEntityStatisticsEntity()
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
                .Group(x => x, entities => new CountStatisticsEntity<EntityType>()
                {
                    Key = entities.Key.EntityType,
                    Count = entities.Key.FavoriteFor.Count
                }).ToListAsync();
            var statistic = new TopEntityStatisticsEntity()
            {
                CountStatistics = statistics
            };
            return statistic;
        }

        public StatisticsType StatisticsType => StatisticsType.TopEntity;

        public TopEntityStatisticsRepository(IMongoCollection<TopEntityStatisticsEntity> collection,
            IMongoCollection<BaseEntity> profileCollection) : base(collection)
        {
            _profileCollection = profileCollection;
        }

        public override async Task UpdateAsync(TopEntityStatisticsEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            entity.LastUpdate = DateTime.UtcNow;
            var result = await _collection.FindOneAndReplaceAsync(x => x.Id == entity.Id, entity);
            if (result == null) await CreateAsync(entity);
        }
    }
}
