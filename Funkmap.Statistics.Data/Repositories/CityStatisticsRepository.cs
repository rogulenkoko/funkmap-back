using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Common.Data.Mongo;
using Funkmap.Data.Entities.Abstract;
using Funkmap.Statistics.Data.Entities;
using Funkmap.Statistics.Data.Repositories.Abstract;
using MongoDB.Driver;
using MongoDB.Driver.GeoJsonObjectModel;

namespace Funkmap.Statistics.Data.Repositories
{
    public class CityStatisticsRepository : MongoRepository<EntityTypeStatisticsEntity>, IProfileStatisticsRepository
    {
        private readonly IMongoCollection<BaseEntity> _profileCollection;
        public StatisticsType StatisticsType => StatisticsType.City;
        public CityStatisticsRepository(IMongoCollection<EntityTypeStatisticsEntity> collection,
            IMongoCollection<BaseEntity> profileCollection) : base(collection)
        {
            _profileCollection = profileCollection;
        }

        public override Task UpdateAsync(EntityTypeStatisticsEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task<BaseStatisticsEntity> BuildFullStatisticsAsync()
        {
            return null;
        }

        public Task<BaseStatisticsEntity> BuildStatisticsAsync(DateTime begin, DateTime end)
        {
            throw new NotImplementedException();
        }

        private string DefineLocation(GeoJsonPoint<GeoJson2DGeographicCoordinates> Location)
        {
            return null;
        }
       
    }
}
