using System;
using System.Threading.Tasks;
using Funkmap.Common.Data.Mongo;
using Funkmap.Data.Entities;
using Funkmap.Data.Repositories.Abstract;
using MongoDB.Driver;

namespace Funkmap.Data.Repositories
{
    public class RehearsalPointRepository : MongoLoginRepository<RehearsalPointEntity>, IRehearsalPointRepository
    {
        public RehearsalPointRepository(IMongoCollection<RehearsalPointEntity> collection) : base(collection)
        {
        }

        public override Task<UpdateResult> UpdateAsync(RehearsalPointEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
