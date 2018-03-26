using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Funkmap.Common.Data.Mongo;
using Funkmap.Statistics.Data.Entities;
using Funkmap.Statistics.Data.Repositories.Abstract;
using MongoDB.Driver;

namespace Funkmap.Statistics.Data.Services
{
    public interface IStatisticsMerger
    {
        Task MergeStatistics();
    }

    public class StatisticsMerger : Repository<BaseStatisticsEntity>, IStatisticsMerger
    {
        private readonly IEnumerable<IStatisticsRepository> _statisticsRepositories;

        public StatisticsMerger(IMongoCollection<BaseStatisticsEntity> collection,
                                IEnumerable<IStatisticsRepository> statisticsRepositories) : base(collection)
        {

            _statisticsRepositories = statisticsRepositories;
        }

        public async Task MergeStatistics()
        {
            var end = DateTime.UtcNow;

            foreach (var statisticsRepository in _statisticsRepositories)
            {
                var typeFilter = Builders<BaseStatisticsEntity>.Filter.Eq(x => x.StatisticsType, statisticsRepository.StatisticsType);
                var saved = await _collection.Find(typeFilter).SingleOrDefaultAsync();
                if (saved == null)
                {
                    saved = await statisticsRepository.BuildFullStatisticsAsync();
                    saved.LastUpdate = end;
                    await  _collection.InsertOneAsync(saved);
                }
                else
                {
                    var begin = saved.LastUpdate;
                    var newStatistics = await statisticsRepository.BuildStatisticsAsync(begin, end);
                    saved.Merge(newStatistics);
                    saved.LastUpdate = end;
                    await _collection.ReplaceOneAsync(typeFilter, saved);
                }

                
            }
        }
        
        public override Task UpdateAsync(BaseStatisticsEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
