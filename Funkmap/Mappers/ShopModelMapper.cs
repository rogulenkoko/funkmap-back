using Funkmap.Data.Entities.Entities;
using Funkmap.Models;
using MongoDB.Bson;
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
                AvatarId = source.PhotoId,
                AvatarMiniId = source.PhotoMiniId,
                VideoInfos = source.VideoInfos,
                SoundCloudTrackIds = source.SoundCloudTrackIds,
                UserLogin = source.UserLogin,
                IsActive = source.IsActive
            };

        }

        public static ShopPreviewModel ToPreviewModel(this ShopEntity source)
        {
            if (source == null) return null;
            return new ShopPreviewModel()
            {
                Login = source.Login,
                Name = source.Name,
                AvatarId = source.PhotoId,
                AvatarMiniId = source.PhotoMiniId,
                VkLink = source.VkLink,
                YoutubeLink = source.YouTubeLink,
                FacebookLink = source.FacebookLink,
                Description = source.Description,
                WorkingHoursDescription = source.WorkingHoursDescription,
                WebSite = source.Website,
                Address = source.Address,
                UserLogin = source.UserLogin,
                IsActive = source.IsActive
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
                VideoInfos = source.VideoInfos,
                SoundCloudTrackIds = source.SoundCloudTrackIds,
                IsActive = source.IsActive,
                UserLogin = source.UserLogin
            };
        }
    }
}
