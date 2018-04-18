using Funkmap.Data.Entities.Entities;
using Funkmap.Domain.Models;
using MongoDB.Driver.GeoJsonObjectModel;

namespace Funkmap.Data.Mappers
{
    public static class ShopModelMapper 
    {
        public static Shop ToModel(this ShopEntity source)
        {
            if (source == null) return null;
            return new Shop
            {
                Location = new Location(source.Location.Coordinates.Latitude, source.Location.Coordinates.Longitude),
                Website = source.Website,
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
                UserLogin = source.UserLogin,
                IsActive = source.IsActive
            };

        }

        public static ShopEntity ToShopEntity(this Shop source)
        {
            if (source == null) return null;
            return new ShopEntity()
            {
                Login = source.Login,
                Description = source.Description,
                FacebookLink = source.FacebookLink,
                Location = source.Location == null
                    ? null
                    : new GeoJsonPoint<GeoJson2DGeographicCoordinates>(new GeoJson2DGeographicCoordinates(source.Location.Longitude, source.Location.Latitude)),
                Name = source.Name,
                SoundCloudLink = source.SoundCloudLink,
                VkLink = source.VkLink,
                YouTubeLink = source.YoutubeLink,
                Website = source.Website,
                WorkingHoursDescription = source.WorkingHoursDescription,
                Address = source.Address,
                IsActive = source.IsActive,
                UserLogin = source.UserLogin
            };
        }
    }
}
