using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Funkmap.Common.Data.Mongo;
using Funkmap.Data.Entities;
using Funkmap.Data.Entities.Entities;
using Funkmap.Data.Repositories.Abstract;
using MongoDB.Driver;

namespace Funkmap.Data.Repositories
{
    public class StudioRepository : MongoLoginRepository<StudioEntity>,IStudioRepository
    {
        public StudioRepository(IMongoCollection<StudioEntity> collection) : base(collection)
        {
        }

        public override async Task<ICollection<StudioEntity>> GetAllAsync()
        {
            var filter = Builders<StudioEntity>.Filter.Eq(x => x.EntityType, EntityType.Studio);
            var result = await _collection.Find(filter).ToListAsync();
            return result;
        }

        public override Task UpdateAsync(StudioEntity entity)
        {
            throw new NotImplementedException("Использовать для обновления BaseRepository");
        }
    }
}
