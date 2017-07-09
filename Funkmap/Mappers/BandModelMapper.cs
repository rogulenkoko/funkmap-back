using System.Linq;
using Funkmap.Data.Entities;
using Funkmap.Models;
using MongoDB.Driver.GeoJsonObjectModel;

namespace Funkmap.Mappers
{
    public static class BandModelMapper
    {
        public static BandModel ToModel(this BandEntity source)
        {
            if (source == null) return null;
            return new BandModel()
            {
                Login = source.Login,
                Longitude = source.Location.Coordinates.Longitude,
                Latitude = source.Location.Coordinates.Latitude,
                Name = source.Name,
                VideoLinks = source.VideoLinks,
                DesiredInstruments = source.DesiredInstruments,
                Musicians = source.MusicianLogins
            };
        }

        public static BandEntity ToBandEntity(this BandModel source)
        {
            if (source == null) return null;

            return new BandEntity()
            {
                Login = source.Login,
                Description = source.Description,
                Location = new GeoJsonPoint<GeoJson2DGeographicCoordinates>(new GeoJson2DGeographicCoordinates(source.Longitude, source.Latitude)),
                Name = source.Name,
                Styles = source.Styles.ToList(),
                Photo = source.Avatar ?? new byte[] { },
                YouTubeLink = source.YouTubeLink,
                VkLink = source.VkLink,
                FacebookLink = source.FacebookLink
            };
        }

        public static BandModelPreview ToModelPreview(this BandEntity source)
        {
            if (source == null) return null;
            return new BandModelPreview()
            {
                Login = source.Login,
                Name = source.Name,
                Avatar = source.Photo?.AsByteArray,
                VkLink = source.VkLink,
                YouTubeLink = source.YouTubeLink,
                FacebookLink = source.FacebookLink,
                DesiredInstruments = source.DesiredInstruments,
                Description = source.Description,
                Styles = source.Styles
            };
        }
    }
}
