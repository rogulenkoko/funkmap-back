using System;
using System.Linq;
using System.Threading.Tasks;
using Funkmap.Data.Entities;
using Funkmap.Data.Entities.Entities;
using Funkmap.Statistics.Data.Entities;
using Funkmap.Statistics.Data.Repositories.Abstract;
using MongoDB.Driver;

namespace Funkmap.Statistics.Data.Repositories
{
    public class InstrumentStatisticsRepository : StatisticsMongoRepository<InstrumentStatisticsEntity>, IMusicianStatisticsRepository
    {
        public StatisticsType StatisticsType => StatisticsType.InstrumentType;

        private readonly IMongoCollection<MusicianEntity> _profileCollection;

        public InstrumentStatisticsRepository(IMongoCollection<InstrumentStatisticsEntity> collection,
            IMongoCollection<MusicianEntity> profileCollection) : base(collection)
        {
            _profileCollection = profileCollection;
        }

        public async Task<BaseStatisticsEntity> BuildFullStatisticsAsync()
        {
            
            /*db.bases.aggregate({$match: {t:1}},
            {$group:
                { _id: "$intsr", count: {$sum: 1} }
            }
            )*/

            var filter = Builders<MusicianEntity>.Filter.Eq(x => x.EntityType, EntityType.Musician);

            var statistics = await _profileCollection.Aggregate()
                .Match(filter)
                .Group(x => x.Instrument, entities => new CountStatisticsEntity<InstrumentType>()
                    {
                        Key = entities.Key,
                        Count = entities.Count()
                    }
                ).ToListAsync();
            var statistic = new InstrumentStatisticsEntity()
            {
                CountStatistics = statistics
            };
            return statistic;
        }

        public Task<BaseStatisticsEntity> BuildStatisticsAsync(DateTime begin, DateTime end)
        {
            return BuildFullStatisticsAsync();
        }

        
    }
}
