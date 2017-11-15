using System;
using System.Threading.Tasks;
using Funkmap.Data.Entities;
using Funkmap.Statistics.Data.Entities;
using Funkmap.Statistics.Data.Repositories.Abstract;
using MongoDB.Driver;

namespace Funkmap.Statistics.Data.Repositories
{
    public class InBandStatisticsRepository : StatisticsMongoRepository<InBandStatisticsEntity>, IMusicianStatisticsRepository
    {
        public StatisticsType StatisticsType => StatisticsType.InBand;

        private readonly IMongoCollection<MusicianEntity> _profileCollection;

        public InBandStatisticsRepository(IMongoCollection<InBandStatisticsEntity> collection, IMongoCollection<MusicianEntity> profileCollection) : base(collection)
        {
            _profileCollection = profileCollection;
        }

        public Task<BaseStatisticsEntity> BuildFullStatisticsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<BaseStatisticsEntity> BuildStatisticsAsync(DateTime begin, DateTime end)
        {
            throw new NotImplementedException();
        }

       
    }
}
