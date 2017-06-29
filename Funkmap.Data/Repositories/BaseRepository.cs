using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Funkmap.Data.Domain;
using Funkmap.Data.Entities;
using Funkmap.Data.Parameters;
using Funkmap.Data.Repositories.Abstract;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Funkmap.Data.Repositories
{
    public class BaseRepository : IBaseRepository
    {
        private readonly IMongoCollection<BaseEntity> _collection;

        public BaseRepository(IMongoCollection<BaseEntity> collection)
        {
            _collection = collection;
        }

        public async Task<ICollection<BaseEntity>> GetAllAsyns()
        {
            var projection = Builders<BaseEntity>.Projection.Exclude(x => x.Photo)
                .Exclude(x => x.Description)
                .Exclude(x => x.Name);


            return await _collection.Find(x => true).Project<BaseEntity>(projection).ToListAsync();
        }

        public async Task<ICollection<BaseEntity>> GetNearestAsync(LocationParameter parameter)
        {

            var center = new[] { parameter.Longitude, parameter.Latitude };
            var centerQueryArray = new BsonArray { new BsonArray(center), parameter.RadiusDeg };
            var filter = new BsonDocument("loc", new BsonDocument("$within", new BsonDocument("$center", centerQueryArray)));
            var projection = Builders<BaseEntity>.Projection.Exclude(x => x.Photo)
                .Exclude(x => x.Description)
                .Exclude(x => x.Name);
            var result = await _collection.Find(filter).Project<BaseEntity>(projection).ToListAsync();
            return result;

        }

        public async Task<ICollection<BaseEntity>> GetFullNearestAsync(FullLocationParameter parameter)
        {
            var center = new[] { parameter.Longitude, parameter.Latitude };
            var centerQueryArray = new BsonArray { new BsonArray(center), parameter.RadiusDeg };
            
            ICollection<BaseEntity> result;
            if (parameter.Longitude == null || parameter.Latitude == null)
            {
                result = await _collection.Find(x => true).Skip(parameter.Skip).Limit(parameter.Take).ToListAsync();
            }
            else
            {
                var filter = new BsonDocument("loc", new BsonDocument("$within", new BsonDocument("$center", centerQueryArray)));
                result = await _collection.Find(filter).Skip(parameter.Skip).Limit(parameter.Take).ToListAsync();
            }

            return result;
        }

        public virtual Task<UpdateResult> UpdateAsync(BaseEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
