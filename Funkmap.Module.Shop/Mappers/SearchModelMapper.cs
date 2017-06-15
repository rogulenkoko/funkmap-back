using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Common;
using Funkmap.Common.Abstract.Search;
using Funkmap.Module.Shop.Models;
using Funkmap.Shop.Data.Entities;

namespace Funkmap.Module.Shop.Mappers
{
    public  static class SearchModelMapper
    {
        public static SearchModel ToSearchModel(this ShopEntity shop)
        {
            if (shop == null)
                return null;
            return new ShopSearchModel()
            {
                Id = shop.Id,
                Longitude = shop.Longitude,
                Latitude = shop.Latitude,
                Name = shop.StoreName,
                ModelType = EntityType.Shop

            };
        }
    }
}
