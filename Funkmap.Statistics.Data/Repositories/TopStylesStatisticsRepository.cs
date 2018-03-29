using System;
using System.Linq;
using System.Threading.Tasks;
using Funkmap.Data.Entities.Entities;
using Funkmap.Domain;
using Funkmap.Domain.Enums;
using Funkmap.Statistics.Data.Entities;
using Funkmap.Statistics.Data.Objects;
using Funkmap.Statistics.Data.Repositories.Abstract;
using MongoDB.Driver;

namespace Funkmap.Statistics.Data.Repositories
{
    public class TopStylesStatisticsRepository : StatisticsRepository<TopStylesStatisticsEntity>, IMusicianStatisticsRepository
    {
        private readonly IMongoCollection<MusicianEntity> _profileCollection;

        public StatisticsType StatisticsType => StatisticsType.TopStyles;

        public TopStylesStatisticsRepository(IMongoCollection<TopStylesStatisticsEntity> collection,
            IMongoCollection<MusicianEntity> profileCollection) : base(collection)
        {
            _profileCollection = profileCollection;
        }

        public async Task<BaseStatisticsEntity> BuildFullStatisticsAsync()
        {
            //db.bases.aggregate(
            //{$unwind: "$stls"},
            //{$project: { stls: 1} },
            //{$group: { _id: "$stls", count: {$sum: 1} } }
            //)

            var filter = Builders<MusicianEntity>.Filter.Eq(x => x.EntityType, EntityType.Musician);

            var projection = Builders<MusicianEntity>.Projection.Include(x => x.Styles);

            var statistics = await _profileCollection.Aggregate()
                .Match(filter)
                .Project<MusicianEntity>(projection)
                .Unwind<MusicianEntity, UnwindStyles>(x => x.Styles)
                .Group(x => x.Style, unwinded => new CountStatisticsEntity<Styles>()
                {
                    Key =  unwinded.Key,
                    Count = unwinded.Count()
                })
                .ToListAsync();

            var statistic = new TopStylesStatisticsEntity()
            {
                CountStatistics = statistics
            };
            return statistic;
        }

        public Task<BaseStatisticsEntity> BuildStatisticsAsync(DateTime begin, DateTime end)
        {
            //запрос по дате не уместен
            return BuildFullStatisticsAsync();
        }

       

        
    }
}
