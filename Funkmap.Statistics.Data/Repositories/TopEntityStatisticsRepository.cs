using System;
using System.Threading.Tasks;
using Funkmap.Data.Entities.Entities.Abstract;
using Funkmap.Statistics.Data.Entities;
using Funkmap.Statistics.Data.Objects;
using Funkmap.Statistics.Data.Repositories.Abstract;
using MongoDB.Driver;

namespace Funkmap.Statistics.Data.Repositories
{
    public class TopEntityStatisticsRepository : StatisticsMongoRepository<TopProfileStatisticsEntity>, IProfileStatisticsRepository
    {
        private readonly IMongoCollection<BaseEntity> _profileCollection;

        public StatisticsType StatisticsType => StatisticsType.TopEntity;


        public TopEntityStatisticsRepository(IMongoCollection<TopProfileStatisticsEntity> collection,
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
            var statistic = new TopProfileStatisticsEntity()
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

       
    }
}
