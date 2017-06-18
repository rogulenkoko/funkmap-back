using System;
using System.Threading.Tasks;
using Funkmap.Common.Data.Mongo;
using Funkmap.Data.Entities;
using Funkmap.Data.Entities.Abstract;
using Funkmap.Data.Repositories.Abstract;
using MongoDB.Driver;

namespace Funkmap.Data.Repositories
{
    public class BandRepository : MongoRepository<BandEntity>, IBandRepository
    {
        private IBandRepository _bandRepositoryImplementation;

        public BandRepository(IMongoCollection<BandEntity> collection) : base(collection)
        {
        }

        public override Task<UpdateResult> UpdateAsync(BandEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
