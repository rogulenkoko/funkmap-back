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
            var projection = Builders<BaseEntity>.Projection.Include(x => x.Login).Include(x=>x.EntityType).Include(x=>x.Location);
            return await _collection.Find(x => true).Project<BaseEntity>(projection).ToListAsync();
        }

        public async Task<ICollection<BaseEntity>> GetNearestAsync(LocationParameter parameter)
        {
            //todo
            var typeDocument = new BsonDocument();
            typeDocument.AddRange(new[]
            {
                new BsonElement("type", "Point"),
                new BsonElement("coordinates", new BsonArray
                {
                    parameter.Longitude,
                    parameter.Latitude
                })
            });
            var geometryElement = new BsonElement("$geometry", typeDocument);
            var nearElement = new BsonDocument();
            nearElement.AddRange(new[]
            {
                geometryElement,
                new BsonElement("$maxDistance", parameter.RadiusDeg)
            });
            var filter = new BsonDocument();
            filter.AddRange(new[]
            {
                new BsonElement("loc", new BsonDocument("$near", nearElement)),
            });

            var result = new List<BaseEntity>();
            using (var cursor = await _collection.FindAsync(filter))
            {
                while (await cursor.MoveNextAsync())
                {
                    var batch = cursor.Current as ICollection<BaseEntity>;
                    if (batch == null) continue;

                    var chunk = new List<BaseEntity>(batch.Count);
                    chunk.AddRange(batch);

                    result.AddRange(chunk);
                }
            }

            return result;
        }

        public virtual Task<UpdateResult> UpdateAsync(BaseEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
