using System;
using System.Threading.Tasks;
using Funkmap.Common.Data.Mongo;
using Funkmap.Data.Entities;
using Funkmap.Data.Repositories.Abstract;
using MongoDB.Driver;

namespace Funkmap.Data.Repositories
{
    public class StudioRepository : MongoLoginRepository<StudioEntity>,IStudioRepository
    {
        public StudioRepository(IMongoCollection<StudioEntity> collection) : base(collection)
        {
        }

        public override Task<UpdateResult> UpdateAsync(StudioEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
