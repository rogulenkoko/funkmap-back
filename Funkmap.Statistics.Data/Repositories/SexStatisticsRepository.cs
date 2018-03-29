using System;
using System.Linq;
using System.Threading.Tasks;
using Funkmap.Data.Entities.Entities;
using Funkmap.Domain;
using Funkmap.Domain.Enums;
using Funkmap.Statistics.Data.Entities;
using Funkmap.Statistics.Data.Repositories.Abstract;
using MongoDB.Driver;

namespace Funkmap.Statistics.Data.Repositories
{
    public class SexStatisticsRepository : StatisticsRepository<SexStatisticsEntity>, IMusicianStatisticsRepository
    {
        private readonly IMongoCollection<MusicianEntity> _profileCollection;

        public StatisticsType StatisticsType => StatisticsType.SexType;

        public SexStatisticsRepository(IMongoCollection<SexStatisticsEntity> collection,
            IMongoCollection<MusicianEntity> profileCollection) : base(collection)
        {
            _profileCollection = profileCollection;
        }

        public async Task<BaseStatisticsEntity> BuildFullStatisticsAsync()
        {
            //db.bases.aggregate({$group: { _id: "$sex", count:{ $sum: 1} } })
            var filter = Builders<MusicianEntity>.Filter.Eq(x => x.EntityType, EntityType.Musician);

            var statistics = await _profileCollection.Aggregate()
                .Match(filter)
                .Group(x => x.Sex, entities => new CountStatisticsEntity<Sex>()
                    {
                        Key = entities.Key,
                        Count = entities.Count()
                    }
                ).ToListAsync();
            var statistic = new SexStatisticsEntity()
            {
                CountStatistics = statistics
            };
            return statistic;
        }

        public async Task<BaseStatisticsEntity> BuildStatisticsAsync(DateTime begin, DateTime end)
        {
            return await BuildFullStatisticsAsync();
            var filter = Builders<MusicianEntity>.Filter.Gte(x => x.CreationDate, begin) &
                         Builders<MusicianEntity>.Filter.Lte(x => x.CreationDate, end) &
                         Builders<MusicianEntity>.Filter.Eq(x => x.EntityType, EntityType.Musician);
            var statistics = await _profileCollection.Aggregate()
                .Match(filter)
                .Group(x => x.Sex, entities => new CountStatisticsEntity<Sex>()
                    {
                        Key = entities.Key,
                        Count = entities.Count()
                    }
                ).ToListAsync();
            var statistic = new SexStatisticsEntity()
            {
                CountStatistics = statistics
            };
            return statistic;
        }

       
    }
}
