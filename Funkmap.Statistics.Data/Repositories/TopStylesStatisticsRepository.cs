using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Common;
using Funkmap.Common.Data.Mongo;
using Funkmap.Data.Entities;
using Funkmap.Data.Entities.Abstract;
using Funkmap.Statistics.Data.Entities;
using Funkmap.Statistics.Data.Repositories.Abstract;
using MongoDB.Driver;

namespace Funkmap.Statistics.Data.Repositories
{
    public class TopStylesStatisticsRepository : MongoRepository<TopStylesStatisticsEntity>, IStatisticsRepository
    {
        private readonly IMongoCollection<MusicianEntity> _profileCollection;
        public async Task<BaseStatisticsEntity> BuildFullStatisticsAsync()
        {
            var statistics = await _profileCollection.Aggregate()
                
                .Group(x => x.Styles, entities => new CountStatisticsEntity<Styles>()
                {
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

        public TopStylesStatisticsRepository(IMongoCollection<TopStylesStatisticsEntity> collection,
            IMongoCollection<MusicianEntity> profileCollection) : base(collection)
        {
            _profileCollection = profileCollection;
        }

        public override async Task UpdateAsync(TopStylesStatisticsEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            entity.LastUpdate = DateTime.UtcNow;
            var result = await _collection.FindOneAndReplaceAsync(x => x.Id == entity.Id, entity);
            if (result == null) await CreateAsync(entity);
        }
    }
}
