using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Common.Data.Mongo;
using Funkmap.Data.Entities;
using Funkmap.Data.Repositories.Abstract;
using MongoDB.Driver;

namespace Funkmap.Data.Repositories
{
    public class ShopRepository : MongoLoginRepository<ShopEntity>, IShopRepository
    {
        public ShopRepository(IMongoCollection<ShopEntity> collection) : base(collection)
        {
        }

        public override Task<UpdateResult> UpdateAsync(ShopEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
