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
    public class InstrumentStatisticsRepository : MongoRepository<InstrumentStatisticsEntity>, IStatisticsRepository
    {
        public StatisticsType StatisticsType => StatisticsType.InstrumentType;
        private readonly IMongoCollection<MusicianEntity> _profileCollection;
        public InstrumentStatisticsRepository(IMongoCollection<InstrumentStatisticsEntity> collection,
            IMongoCollection<MusicianEntity> profileCollection) : base(collection)
        {
            _profileCollection = profileCollection;
        }

        public override async Task UpdateAsync(InstrumentStatisticsEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            entity.LastUpdate = DateTime.UtcNow;
            var result = await _collection.FindOneAndReplaceAsync(x => x.Id == entity.Id, entity);
            if (result == null) await CreateAsync(entity);
        }

        public async Task<BaseStatisticsEntity> BuildFullStatisticsAsync()
        {
            
            /*db.bases.aggregate({$match: {t:1}},
            {$group:
                { _id: "$intsr", count: {$sum: 1} }
            }
            )*/
            var statistics = await _profileCollection.Aggregate()
                .Match(x=>x.Instrument!=null)
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

        public async Task<BaseStatisticsEntity> BuildStatisticsAsync(DateTime begin, DateTime end)
        {
            var filter = Builders<MusicianEntity>.Filter.Gte(x => x.CreationDate, begin) &
                         Builders<MusicianEntity>.Filter.Lte(x => x.CreationDate, end);
            var statistics = await _profileCollection.Aggregate()
                .Match(filter )
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

        
    }
}
