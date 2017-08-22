using System;
using Funkmap.Data.Entities;
using Funkmap.Models;
using MongoDB.Driver.GeoJsonObjectModel;

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
                WebSite = sourse.Website,
                Login = sourse.Login,
                Name = sourse.Name,
                Description = sourse.Description,
                YouTubeLink = sourse.YouTubeLink,
                VkLink = sourse.VkLink,
                FacebookLink = sourse.FacebookLink,
                SoundCloudLink = sourse.SoundCloudLink
            };

        }

        public static ShopPreviewModel ToPreviewModel(this ShopEntity source)
        {
            if (source == null) return null;
            return new ShopPreviewModel()
            {
                Login = source.Login,
                Name = source.Name,
                Avatar = source.Photo?.AsByteArray,
                VkLink = source.VkLink,
                YouTubeLink = source.YouTubeLink,
                FacebookLink = source.FacebookLink,
                Description = source.Description,
                WorkingHoursDescription = source.WorkingHoursDescription,
                WebSite = source.Website,
                Address = source.Address
            };
        }

        public static ShopEntity ToShopEntity(this ShopModel source)
        {
            if (source == null) return null;
            return new ShopEntity()
            {
                Login = source.Login,
                Description = source.Description,
                FacebookLink = source.FacebookLink,
                Location = source.Longitude != 0 && source.Latitude != 0 ? new GeoJsonPoint<GeoJson2DGeographicCoordinates>(new GeoJson2DGeographicCoordinates(source.Longitude, source.Latitude)) : null,
                Name = source.Name,
                SoundCloudLink = source.SoundCloudLink,
                VkLink = source.VkLink,
                YouTubeLink = source.YouTubeLink,
                Website = source.WebSite,
                WorkingHoursDescription = source.WorkingHoursDescription
            };
        }
    }
}
