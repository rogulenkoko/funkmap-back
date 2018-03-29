using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Funkmap.Data.Entities.Entities.Abstract;
using Funkmap.Domain.Abstract;
using MongoDB.Driver;

namespace Funkmap.Tests.Funkmap.Tools
{
    internal class TestToolRepository
    {
        private readonly IMongoCollection<BaseEntity> _collection;

        public TestToolRepository(IMongoCollection<BaseEntity> collection)
        {
            _collection = collection;
        }

        public async Task<List<string>> GetAnyLoginsAsync(long? count = null)
        {
            var filter = Builders<BaseEntity>.Filter.Empty;

            List<BaseEntity> some;

            if (!count.HasValue)
            {
                some = await _collection.Find(filter).ToListAsync();
            }
            else
            {
                some = await _collection.Find(filter).Limit((int)count.Value).ToListAsync();
            }
            
            return some.Select(x => x.Login).ToList();
        }

        public DistanceResult GetDistances(ILocationParameter parameter)
        {
            var longitude = parameter.Longitude.Value.ToString(CultureInfo.InvariantCulture);
            var latitude = parameter.Latitude.Value.ToString(CultureInfo.InvariantCulture);
            var command = new JsonCommand<DistanceResult>($"{{ geoNear: 'bases',near: [ { longitude }, { latitude } ], spherical: true}}");
            return _collection.Database.RunCommand(command);
        }

        public async Task<List<string>> GetProfileUsersAsync()
        {
            var projection = Builders<BaseEntity>.Projection.Include(x => x.UserLogin);
            var entities = await _collection.Find(Builders<BaseEntity>.Filter.Empty).Project<BaseEntity>(projection).ToListAsync();
            return entities.Select(x => x.UserLogin).ToList();
        }
    }
}
