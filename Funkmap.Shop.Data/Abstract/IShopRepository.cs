using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Common.Data.Abstract;
using Funkmap.Common.Data.Parameters;
using Funkmap.Shop.Data.Entities;

namespace Funkmap.Shop.Data.Abstract
{
    public interface IShopRepository : IRepository<ShopEntity>
    {
        Task<ICollection<ShopEntity>> GetShopsPreviews();
        Task<ICollection<ShopEntity>> GetNearestShopsPreviews(LocationParameter currentLocation);
        Task<ICollection<ShopEntity>> GetShopsPreviewsSearchByName(string name);
    }
}
