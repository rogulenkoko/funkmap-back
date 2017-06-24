using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Funkmap.Data.Entities.Abstract;
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
            var projection = Builders<BaseEntity>.Projection.Include(x => x.Login).Include(x=>x.EntityType).Include(x=>x.Location).Include(x=>x.Instrument);
            return await _collection.Find(x => true).Project<BaseEntity>(projection).ToListAsync();
        }

        public async Task<ICollection<BaseEntity>> GetNearestAsync(LocationParameter parameter)
        {

            var center = new [] { parameter.Longitude, parameter.Latitude };
            var centerQueryArray = new BsonArray { new BsonArray(center), parameter.RadiusDeg };
            var filter = new BsonDocument("loc", new BsonDocument("$within", new BsonDocument("$center", centerQueryArray)));
            var projection = Builders<BaseEntity>.Projection.Include(x => x.Login).Include(x => x.EntityType).Include(x => x.Location).Include(x => x.Instrument);
            var result = await _collection.Find(filter).Project<BaseEntity>(projection).ToListAsync();
            return result;

        }

        public virtual Task<UpdateResult> UpdateAsync(BaseEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
