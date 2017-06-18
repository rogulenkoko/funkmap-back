using Funkmap.Common.Data.Mongo;
using Funkmap.Common.Data.Mongo.Abstract;
using Funkmap.Data.Entities;
using Funkmap.Data.Entities.Abstract;

namespace Funkmap.Data.Repositories.Abstract
{
    public interface IBandRepository : IMongoRepository<BandEntity>
    {
    }
}
