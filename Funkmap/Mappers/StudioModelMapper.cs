using Funkmap.Data.Entities.Entities;
using Funkmap.Models;
using MongoDB.Driver.GeoJsonObjectModel;

namespace Funkmap.Mappers
{
    public static class StudioModelMapper
    {

        public static StudioModel ToModel(this StudioEntity source)
        {
            if (source == null) return null;
            return new StudioModel()
            {
                Login = source.Login,
                Name = source.Name,
                AvatarId = source.PhotoId,
                AvatarMiniId = source.PhotoMiniId,
                VkLink = source.VkLink,
                YoutubeLink = source.YouTubeLink,
                FacebookLink = source.FacebookLink,
                WorkingHoursDescription = source.WorkingHoursDescription,
                Description = source.Description,
                Address = source.Address,
                Longitude = source.Location.Coordinates.Longitude,
                Latitude = source.Location.Coordinates.Latitude,
                SoundCloudLink = source.SoundCloudLink,
                VideoInfos = source.VideoInfos,
                SoundCloudTrackIds = source.SoundCloudTrackIds,
                UserLogin = source.UserLogin,
                IsActive = source.IsActive
            };
        }


        public static StudioPreviewModel ToPreviewModel(this StudioEntity source)
        {
            if (source == null) return null;
            return new StudioPreviewModel()
            {
                Login = source.Login,
                Name = source.Name,
                AvatarId = source.PhotoId,
                AvatarMiniId = source.PhotoMiniId,
                VkLink = source.VkLink,
                YoutubeLink = source.YouTubeLink,
                FacebookLink = source.FacebookLink,
                WorkingHoursDescription = source.WorkingHoursDescription,
                Description = source.Description,
                Address = source.Address,
                SoundCloudLink = source.SoundCloudLink,
                UserLogin = source.UserLogin,
                IsActive = source.IsActive
            };
        }

        public static StudioEntity ToStudioEntity(this StudioModel source)
        {
            if (source == null) return null;
            return new StudioEntity()
            {
                Login = source.Login,
                FacebookLink = source.FacebookLink,
                Description = source.Description,
                Location = source.Longitude != 0 && source.Latitude != 0 ? new GeoJsonPoint<GeoJson2DGeographicCoordinates>(new GeoJson2DGeographicCoordinates(source.Longitude, source.Latitude)) : null,
                Name = source.Name,
                SoundCloudLink = source.SoundCloudLink,
                VkLink = source.VkLink,
                YouTubeLink = source.YoutubeLink,
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
