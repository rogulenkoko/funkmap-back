using System;
using Funkmap.Data.Entities;
using Funkmap.Data.Entities.Abstract;
using Funkmap.Models;
using MongoDB.Driver.GeoJsonObjectModel;

namespace Funkmap.Mappers
{
    public static class ShopModelMapper 
    {
        public static ShopModel ToModel(this ShopEntity source)
        {
            if (source == null) return null;
            return new ShopModel
            {
                StoreName = source.Name,
                Latitude = source.Location.Coordinates.Latitude,
                Longitude = source.Location.Coordinates.Longitude,
                WebSite = source.Website,
                Login = source.Login,
                Name = source.Name,
                Description = source.Description,
                YoutubeLink = source.YouTubeLink,
                VkLink = source.VkLink,
                FacebookLink = source.FacebookLink,
                SoundCloudLink = source.SoundCloudLink,
                Address = source.Address,
                Avatar = source.Photo?.Image?.AsByteArray,
                VideoInfos = source.VideoInfos,
                UserLogin = source.UserLogin
            };

        }

        public static ShopPreviewModel ToPreviewModel(this ShopEntity source)
        {
            if (source == null) return null;
            return new ShopPreviewModel()
            {
                Login = source.Login,
                Name = source.Name,
                Avatar = source.Photo?.Image?.AsByteArray,
                VkLink = source.VkLink,
                YoutubeLink = source.YouTubeLink,
                FacebookLink = source.FacebookLink,
                Description = source.Description,
                WorkingHoursDescription = source.WorkingHoursDescription,
                WebSite = source.Website,
                Address = source.Address,
                UserLogin = source.UserLogin
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
                YouTubeLink = source.YoutubeLink,
                Website = source.WebSite,
                WorkingHoursDescription = source.WorkingHoursDescription,
                Address = source.Address,
                Photo = source.Avatar == null ? null : new ImageInfo() { Image = source.Avatar },
                VideoInfos = source.VideoInfos
            };
        }
    }
}
