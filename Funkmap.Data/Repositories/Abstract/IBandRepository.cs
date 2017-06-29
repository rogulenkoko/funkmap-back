using Funkmap.Common.Data.Mongo;
using Funkmap.Common.Data.Mongo.Abstract;
using Funkmap.Data.Entities;

namespace Funkmap.Data.Repositories.Abstract
{
    public interface IBandRepository : IMongoRepository<BandEntity>
    {
    }
}
