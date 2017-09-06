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
                Musicians = source.MusicianLogins,
                FacebookLink = source.FacebookLink,
                SoundCloudLink = source.SoundCloudLink,
                YoutubeLink = source.YouTubeLink,
                VkLink = source.VkLink,
                Styles = source.Styles,
                Description = source.Description,
                Avatar = source.Photo?.AsByteArray,
                Address = source.Address,
                VideoInfos = source.VideoInfos
            };
        }

        public static BandEntity ToBandEntity(this BandModel source)
        {
            if (source == null) return null;

            return new BandEntity()
            {
                Login = source.Login,
                Description = source.Description,
                Location = source.Longitude != 0 && source.Latitude != 0 ? new GeoJsonPoint<GeoJson2DGeographicCoordinates>(new GeoJson2DGeographicCoordinates(source.Longitude, source.Latitude)) : null,
                Name = source.Name,
                Styles = source.Styles?.ToList(),
                Photo = source.Avatar ?? new byte[] { },
                YouTubeLink = source.YoutubeLink,
                VkLink = source.VkLink,
                FacebookLink = source.FacebookLink,
                SoundCloudLink = source.SoundCloudLink,
                Address = source.Address,
                VideoInfos = source.VideoInfos
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
                YoutubeLink = source.YouTubeLink,
                FacebookLink = source.FacebookLink,
                DesiredInstruments = source.DesiredInstruments,
                Description = source.Description,
                Styles = source.Styles,
                SoundCloudLink = source.SoundCloudLink
                
            };
        }
    }
}
