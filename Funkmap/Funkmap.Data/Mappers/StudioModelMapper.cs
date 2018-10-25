using Funkmap.Data.Entities.Entities;
using Funkmap.Domain.Models;
using MongoDB.Driver.GeoJsonObjectModel;

namespace Funkmap.Data.Mappers
{
    public static class StudioModelMapper
    {

        public static Studio ToModel(this StudioEntity source)
        {
            if (source == null) return null;
            return new Studio()
            {
                Login = source.Login,
                Name = source.Name,
                AvatarUrl = source.AvatarUrl,
                AvatarMiniUrl = source.AvatarMiniUrl,
                VkLink = source.VkLink,
                YoutubeLink = source.YouTubeLink,
                FacebookLink = source.FacebookLink,
                WorkingHoursDescription = source.WorkingHoursDescription,
                Description = source.Description,
                Address = source.Address,
                Location = new Location(source.Location.Coordinates.Latitude, source.Location.Coordinates.Longitude),
                SoundCloudLink = source.SoundCloudLink,
                UserLogin = source.UserLogin,
                IsActive = source.IsActive,
                IsPriority = source.IsPriority
            };
        }

        public static StudioEntity ToStudioEntity(this Studio source)
        {
            if (source == null) return null;
            return new StudioEntity()
            {
                Login = source.Login,
                FacebookLink = source.FacebookLink,
                Description = source.Description,
                Location = source.Location == null
                    ? null
                    : new GeoJsonPoint<GeoJson2DGeographicCoordinates>(new GeoJson2DGeographicCoordinates(source.Location.Longitude, source.Location.Latitude)),
                Name = source.Name,
                SoundCloudLink = source.SoundCloudLink,
                VkLink = source.VkLink,
                YouTubeLink = source.YoutubeLink,
                WorkingHoursDescription = source.WorkingHoursDescription,
                Address = source.Address,
                IsActive = source.IsActive,
                UserLogin = source.UserLogin
            };
        }
    }
}
