using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Common.Data;
using Funkmap.Common.Data.Parameters;
using Funkmap.Shop.Data.Abstract;
using Funkmap.Shop.Data.Entities;

namespace Funkmap.Shop.Data.Repositories
{
    public class ShopRepository : Repository<ShopEntity>, IShopRepository
    {
        public ShopRepository(ShopContext context) : base(context)
        {
        }

        public async Task<ICollection<ShopEntity>> GetShopsPreviews()
        {
            var shopPreviewsObj = await Context.Set<ShopEntity>().Select(x => new
            {
                Latitude = x.Latitude,
                StoreName = x.StoreName,
                Longitude = x.Longitude,
                Id = x.Id
            }).ToListAsync();

            var shopPreviews = shopPreviewsObj.Select(x => new ShopEntity()
            {
                Id = x.Id,
                StoreName = x.StoreName,
                Latitude = x.Latitude,
                Longitude = x.Longitude
            }).ToList();
            return shopPreviews;
        }

        public async Task<ICollection<ShopEntity>> GetNearestShopsPreviews(LocationParameter currentLocation)
        {
            var query = Context.Set<ShopEntity>().Select(x => x);
            if (currentLocation != null)
            {
                query = query.Where(x => x.Longitude <= currentLocation.Longitude + currentLocation.RadiusDeg
                                         && x.Longitude >= currentLocation.Longitude - currentLocation.RadiusDeg
                                         && x.Latitude <= currentLocation.Latitude + currentLocation.RadiusDeg
                                         && x.Latitude >= currentLocation.Latitude - currentLocation.RadiusDeg);
            }

            var shopPreviews = await SelectPreviews(query);

            return shopPreviews;
        }
        private async Task<ICollection<ShopEntity>> SelectPreviews(IQueryable<ShopEntity> query)
        {
            var shops = await query.Select(x => new
            {
                Latitude = x.Latitude,
                StoreName = x.StoreName,
                Longitude = x.Longitude,
                Id = x.Id
            }).ToListAsync();

            return shops.Select(x => new ShopEntity()
            {
                Latitude = x.Latitude,
                StoreName = x.StoreName,
                Longitude = x.Longitude,
                Id = x.Id,
            }).ToList();
        }
    

        public async Task<ICollection<ShopEntity>> GetShopsPreviewsSearchByName(string name)
        {
            var query = Context.Set<ShopEntity>().Select(x => x);
            if (!String.IsNullOrEmpty(name))
            {
                query = query.Where(x => x.StoreName.Equals(name));
            }
            var shopPreviews = await SelectPreviews(query);
            return shopPreviews;

        }
    }
}
