using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Funkmap.Common;
using Funkmap.Common.Data.Mongo;
using Funkmap.Data.Entities;
using Funkmap.Data.Repositories.Abstract;
using MongoDB.Driver;

namespace Funkmap.Data.Repositories
{
    public class BandRepository : MongoLoginRepository<BandEntity>, IBandRepository
    {
        public BandRepository(IMongoCollection<BandEntity> collection) : base(collection)
        {
        }

        public override async Task<ICollection<BandEntity>> GetAllAsync()
        {
            var filter = Builders<BandEntity>.Filter.Eq(x => x.EntityType, EntityType.Band);
            var result = await _collection.Find(filter).ToListAsync();
            return result;
        }

        public override Task UpdateAsync(BandEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
