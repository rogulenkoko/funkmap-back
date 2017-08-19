using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Data.Entities;
using Funkmap.Models;
using MongoDB.Driver.GeoJsonObjectModel;

namespace Funkmap.Mappers
{
    public static class RehearsalPointMapper
    {
        public static RehearsalPointPreviewModel ToPreviewModel(this RehearsalPointEntity source)
        {
            if (source == null) return null;
            return new RehearsalPointPreviewModel()
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
                Location = new GeoJsonPoint<GeoJson2DGeographicCoordinates>(new GeoJson2DGeographicCoordinates(source.Longitude, source.Latitude)),
                Name = source.Name,
                YouTubeLink = source.YouTubeLink
            };
        }
    }
}
