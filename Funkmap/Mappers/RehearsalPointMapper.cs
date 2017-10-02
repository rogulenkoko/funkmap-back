using Funkmap.Data.Entities;
using Funkmap.Data.Entities.Abstract;
using Funkmap.Models;
using MongoDB.Driver.GeoJsonObjectModel;

namespace Funkmap.Mappers
{
    public static class RehearsalPointMapper
    {

        public static RehearsalPointModel ToModel(this RehearsalPointEntity source)
        {
            if (source == null) return null;
            return new RehearsalPointModel()
            {
                Login = source.Login,
                Name = source.Name,
                Avatar = source.Photo?.Image?.AsByteArray,
                VkLink = source.VkLink,
                YoutubeLink = source.YouTubeLink,
                FacebookLink = source.FacebookLink,
                WorkingHoursDescription = source.WorkingHoursDescription,
                Description = source.Description,
                Address = source.Address,
                SoundCloudLink = source.SoundCloudLink,
                Longitude = source.Location.Coordinates.Longitude,
                Latitude = source.Location.Coordinates.Latitude,
                VideoInfos = source.VideoInfos,
                UserLogin = source.UserLogin,
                IsActive = source.IsActive
            };
        }

        public static RehearsalPointPreviewModel ToPreviewModel(this RehearsalPointEntity source)
        {
            if (source == null) return null;
            return new RehearsalPointPreviewModel()
            {
                Login = source.Login,
                Name = source.Name,
                Avatar = source.Photo?.Image?.AsByteArray,
                VkLink = source.VkLink,
                YoutubeLink = source.YouTubeLink,
                FacebookLink = source.FacebookLink,
                WorkingHoursDescription = source.WorkingHoursDescription,
                Description = source.Description,
                Address = source.Address,
                UserLogin = source.UserLogin,
                IsActive = source.IsActive
            };
        }

        public static RehearsalPointEntity ToRehearsalPointEntity(this RehearsalPointModel source)
        {
            if (source == null) return null;
            return new RehearsalPointEntity()
            {
                Login = source.Login,
                Description = source.Description,
                FacebookLink = source.FacebookLink,
                SoundCloudLink = source.SoundCloudLink,
                VkLink = source.VkLink,
                Location = source.Longitude != 0 && source.Latitude != 0 ? new GeoJsonPoint<GeoJson2DGeographicCoordinates>(new GeoJson2DGeographicCoordinates(source.Longitude, source.Latitude)) : null,
                Name = source.Name,
                YouTubeLink = source.YoutubeLink,
                Address = source.Address,
                Photo = source.Avatar == null ? null : new ImageInfo() { Image = source.Avatar },
                VideoInfos = source.VideoInfos,
                IsActive = source.IsActive,
                UserLogin = source.UserLogin
            };
        }
    }
}
