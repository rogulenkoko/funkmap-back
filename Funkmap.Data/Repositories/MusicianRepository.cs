using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Funkmap.Common;
using Funkmap.Common.Data.Mongo;
using Funkmap.Data.Entities;
using Funkmap.Data.Parameters;
using Funkmap.Data.Repositories.Abstract;
using MongoDB.Driver;

namespace Funkmap.Data.Repositories
{
    public class MusicianRepository : MongoLoginRepository<MusicianEntity>, IMusicianRepository
    {
        private readonly IMongoCollection<MusicianEntity> _collection;

        public MusicianRepository(IMongoCollection<MusicianEntity> collection) : base(collection)
        {
            _collection = collection;
        }

        public override async Task UpdateAsync(MusicianEntity entity)
        {
            var filter = Builders<MusicianEntity>.Filter.Eq(x => x.Id, entity.Id);

            await _collection.ReplaceOneAsync(filter, entity);
        }

        public override async Task<ICollection<MusicianEntity>> GetAllAsync()
        {
            var filter = Builders<MusicianEntity>.Filter.Eq(x => x.EntityType, EntityType.Musician);
            var result = await _collection.Find(filter).ToListAsync();
            return result;
        }
    }
}
