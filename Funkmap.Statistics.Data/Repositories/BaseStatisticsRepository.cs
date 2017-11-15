using System.Collections.Generic;
using System.Threading.Tasks;
using Funkmap.Statistics.Data.Entities;
using Funkmap.Statistics.Data.Repositories.Abstract;
using MongoDB.Driver;

namespace Funkmap.Statistics.Data.Repositories
{
    public class BaseStatisticsRepository : IBaseStatisticsRepository
    {
        private readonly IMongoCollection<BaseStatisticsEntity> _collection;

        public BaseStatisticsRepository(IMongoCollection<BaseStatisticsEntity> collection)
        {
            _collection = collection;
        }


        public async Task<ICollection<BaseStatisticsEntity>> GetAllStatisticsAsync(StatisticsType[] statisticsTypes = null)
        {

            var filter = Builders<BaseStatisticsEntity>.Filter.Empty;

            if (statisticsTypes != null && statisticsTypes.Length > 0)
            {
                filter = filter & Builders<BaseStatisticsEntity>.Filter.In(x => x.StatisticsType, statisticsTypes);
            }

            return await _collection.Find(filter).ToListAsync();
        }
    }
}
