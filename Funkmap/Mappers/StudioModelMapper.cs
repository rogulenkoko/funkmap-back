using Funkmap.Data.Entities;
using Funkmap.Models;
using MongoDB.Driver.GeoJsonObjectModel;

namespace Funkmap.Mappers
{
    public static class StudioModelMapper
    {
        public static StudioPreviewModel ToPreviewModel(this StudioEntity source)
        {
            if (source == null) return null;
            return new StudioPreviewModel()
            {
                Login = source.Login,
                Name = source.Name,
                Avatar = source.Photo?.AsByteArray,
                VkLink = source.VkLink,
                YouTubeLink = source.YouTubeLink,
                FacebookLink = source.FacebookLink,
                WorkingHoursDescription = source.WorkingHoursDescription,
                Description = source.Description,
                Address = source.Address
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
                Location = new GeoJsonPoint<GeoJson2DGeographicCoordinates>(new GeoJson2DGeographicCoordinates(source.Longitude, source.Latitude)),
                Name = source.Name,
                SoundCloudLink = source.SoundCloudLink,
                VkLink = source.VkLink,
                YouTubeLink = source.YouTubeLink,
                WorkingHoursDescription = source.WorkingHoursDescription
            };
        }
    }
}
