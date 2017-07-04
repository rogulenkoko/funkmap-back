using System;
using System.Threading.Tasks;
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

        public override Task<UpdateResult> UpdateAsync(BandEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
