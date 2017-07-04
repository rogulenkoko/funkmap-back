using Funkmap.Data.Entities;
using Funkmap.Models;

namespace Funkmap.Mappers
{
    public static class ShopModelMapper 
    {
        public static ShopModel ToModel(this ShopEntity sourse)
        {
            if (sourse == null) return null;
            return new ShopModel
            {
                StoreName = sourse.Name,
                Latitude = sourse.Location.Coordinates.Latitude,
                Longitude = sourse.Location.Coordinates.Longitude,
                WebSite = sourse.Website
            };

        }
    }
}
