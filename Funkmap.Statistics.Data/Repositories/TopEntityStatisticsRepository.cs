using System;
using System.Threading.Tasks;
using Funkmap.Common.Data.Mongo;
using Funkmap.Data.Entities.Abstract;
using Funkmap.Statistics.Data.Entities;
using Funkmap.Statistics.Data.Objects;
using Funkmap.Statistics.Data.Repositories.Abstract;
using MongoDB.Driver;

namespace Funkmap.Statistics.Data.Repositories
{
    public class TopEntityStatisticsRepository : MongoRepository<TopEntityStatisticsEntity>, IProfileStatisticsRepository
    {
        private readonly IMongoCollection<BaseEntity> _profileCollection;

        public TopEntityStatisticsRepository(IMongoCollection<TopEntityStatisticsEntity> collection,
                                             IMongoCollection<BaseEntity> profileCollection) : base(collection)
        {
            _profileCollection = profileCollection;
        }

        public async Task<BaseStatisticsEntity> BuildFullStatisticsAsync()
        {
            //db.bases.aggregate(
            //{$project: { login: "$log", type: "$t", count: { "$ifNull":["$fav", 0]}}},
            //{$sort: {count: 1}},
            //{$limit:5}
            //)

            var sort = Builders<TopEntityStatistic>.Sort.Descending(x => x.Count);
            var filter = Builders<BaseEntity>.Filter.Ne(x => x.FavoriteFor, null);
            var statistics = await _profileCollection.Aggregate()
                .Match(filter)
                .Project(entity => new TopEntityStatistic()
                {
                    Login = entity.Login,
                    Id = entity.Id,
                    EntityType = entity.EntityType,
                    Count = entity.FavoriteFor.Count
                })
                .Sort(sort)
                .Limit(5)
                .ToListAsync();
            var statistic = new TopEntityStatisticsEntity()
            {
                CountStatistics = statistics
            };
            return statistic;
        }

        public Task<BaseStatisticsEntity> BuildStatisticsAsync(DateTime begin, DateTime end)
        {
            //тут не уместна выборка по дате поэтому отдаем независимо от даты
            return BuildFullStatisticsAsync();
        }

        public StatisticsType StatisticsType => StatisticsType.TopEntity;
        

        public override async Task UpdateAsync(TopEntityStatisticsEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            entity.LastUpdate = DateTime.UtcNow;
            var result = await _collection.FindOneAndReplaceAsync(x => x.Id == entity.Id, entity);
            if (result == null) await CreateAsync(entity);
        }
    }
}
