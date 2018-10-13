using System.Linq;
using Funkmap.Data.Entities.Entities;
using Funkmap.Domain.Models;
using MongoDB.Driver.GeoJsonObjectModel;

namespace Funkmap.Data.Mappers
{
    public static class BandModelMapper
    {
        public static Band ToModel(this BandEntity source)
        {
            if (source == null) return null;
            return new Band
            {
                Login = source.Login,
                Location = new Location(source.Location.Coordinates.Latitude, source.Location.Coordinates.Longitude),
                Name = source.Name,
                DesiredInstruments = source.DesiredInstruments,
                Musicians = source.MusicianLogins,
                InvitedMusicians = source.InvitedMusicians,
                FacebookLink = source.FacebookLink,
                SoundCloudLink = source.SoundCloudLink,
                YoutubeLink = source.YouTubeLink,
                VkLink = source.VkLink,
                Styles = source.Styles,
                Description = source.Description,
                AvatarUrl = source.AvatarUrl,
                AvatarMiniUrl = source.AvatarMiniUrl,
                Address = source.Address,
                VideoInfos = source.VideoInfos?.Select(x=>x.ToModel()).ToList(),
                SoundCloudTracks = source.SoundCloudTracks?.Select(x=>x.ToModel()).ToList(),
                UserLogin = source.UserLogin,
                IsActive = source.IsActive
            };
        }

        public static BandEntity ToBandEntity(this Band source)
        {
            if (source == null) return null;

            return new BandEntity()
            {
                Login = source.Login,
                Description = source.Description,
                Location = source.Location == null
                    ? null
                    : new GeoJsonPoint<GeoJson2DGeographicCoordinates>(new GeoJson2DGeographicCoordinates(source.Location.Longitude, source.Location.Latitude)),
                Name = source.Name,
                Styles = source.Styles,
                YouTubeLink = source.YoutubeLink,
                VkLink = source.VkLink,
                FacebookLink = source.FacebookLink,
                SoundCloudLink = source.SoundCloudLink,
                Address = source.Address,
                VideoInfos = source.VideoInfos?.Select(x=>x.ToEntity()).ToList(),
                SoundCloudTracks = source.SoundCloudTracks?.Select(x=>x.ToEntity()).ToList(),
                IsActive = source.IsActive,
                UserLogin = source.UserLogin,
                MusicianLogins = source.Musicians
            };
        }
    }
}
