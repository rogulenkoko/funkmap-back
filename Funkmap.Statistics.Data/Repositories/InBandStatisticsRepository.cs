using System;
using System.Linq;
using System.Threading.Tasks;
using Funkmap.Common;
using Funkmap.Data.Entities;
using Funkmap.Statistics.Data.Entities;
using Funkmap.Statistics.Data.Objects;
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

        public async Task<BaseStatisticsEntity> BuildFullStatisticsAsync()
        {
           
            var filter = Builders<MusicianEntity>.Filter.Eq(x => x.EntityType, EntityType.Musician) &
                         Builders<MusicianEntity>.Filter.Ne(x => x.BandLogins, null);



            var statistics = await _profileCollection.Aggregate()
                .Match(filter)
                .Group(x => x.BandLogins.Count>0 , unwinded => new CountStatisticsEntity<bool>()
                {
                    Key = unwinded.Key,
                    Count = unwinded.Count()
                })
                .ToListAsync();

            var statistic = new InBandStatisticsEntity()
            {
                CountStatistics = statistics
            };
            return statistic;
            //var filter = Builders<MusicianEntity>.Filter.Ne(x => x.BandLogins, null) & Builders<MusicianEntity>.Filter.
        }

        public async Task<BaseStatisticsEntity> BuildStatisticsAsync(DateTime begin, DateTime end)
        {
            var filter = Builders<MusicianEntity>.Filter.Eq(x => x.EntityType, EntityType.Musician) &
                         Builders<MusicianEntity>.Filter.Gte(x => x.CreationDate, begin) &
                         Builders<MusicianEntity>.Filter.Lte(x => x.CreationDate, end)&
                         Builders<MusicianEntity>.Filter.Ne(x => x.BandLogins, null);



            var statistics = await _profileCollection.Aggregate()
                .Match(filter)
                .Group(x => x.BandLogins.Count > 0, unwinded => new CountStatisticsEntity<bool>()
                {
                    Key = unwinded.Key,
                    Count = unwinded.Count()
                })
                .ToListAsync();

            var statistic = new InBandStatisticsEntity()
            {
                CountStatistics = statistics
            };
            return statistic;
        }

       
    }
}
