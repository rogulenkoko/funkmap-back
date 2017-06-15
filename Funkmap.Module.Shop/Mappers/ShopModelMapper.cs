using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Module.Shop.Models;
using Funkmap.Shop.Data.Entities;

namespace Funkmap.Module.Shop.Mappers
{
    public static class ShopModelMapper 
    {
        public static ShopModel ToModel(this ShopEntity sourse)
        {
            if (sourse == null)
                return null;
            return new ShopModel
            {
                StoreName = sourse.StoreName,
                Latitude = sourse.Latitude,
                Longitude = sourse.Longitude,
                URLShop = sourse.URLShop
            };

        }
    }
}
