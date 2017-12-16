using Funkmap.Common.Data.Mongo.Abstract;
using Funkmap.Data.Entities.Entities;

namespace Funkmap.Data.Repositories.Abstract
{
    public interface IShopRepository : IMongoRepository<ShopEntity>
    {
    }
}
