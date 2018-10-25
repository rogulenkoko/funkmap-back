using System;
using System.Linq;
using Funkmap.Data.Entities.Entities;
using Funkmap.Domain.Models;
using MongoDB.Driver.GeoJsonObjectModel;

namespace Funkmap.Data.Mappers
{
    public static class MusicianModelMapper
    {
        public static Musician ToModel(this MusicianEntity source)
        {
            if (source == null) return null;
            int? age = source.BirthDate == null ? 0 : (int)Math.Floor((DateTime.Now - source.BirthDate.Value).TotalDays / 365);
            return new Musician()
            {
                Login = source.Login,
                Description = source.Description,
                Location = new Location(source.Location.Coordinates.Latitude, source.Location.Coordinates.Longitude),
                Name = source.Name,
                Sex = source.Sex,
                BirthDate = source.BirthDate,
                Age = age.Value == 0 ? null : age,
                Styles = source.Styles,
                AvatarUrl = source.AvatarUrl,
                AvatarMiniUrl = source.AvatarMiniUrl,
                VideoInfos = source.VideoInfos?.Select(x=>x.ToModel()).ToList(),
                SoundCloudTracks = source.SoundCloudTracks?.Select(x=>x.ToModel()).ToList(),
                VkLink = source.VkLink,
                FacebookLink = source.FacebookLink,
                Instrument = source.Instrument,
                Expirience = source.ExpirienceType,
                SoundCloudLink = source.SoundCloudLink,
                YoutubeLink = source.YouTubeLink,
                Address = source.Address,
                UserLogin = source.UserLogin,
                IsActive = source.IsActive,
                BandLogins = source.BandLogins,
                IsPriority = source.IsPriority
            };
        }

        public static MusicianEntity ToMusicianEntity(this Musician source)
        {
            if (source == null) return null;
            
            return new MusicianEntity
            {
                Login = source.Login,
                Description = source.Description,
                Location = source.Location == null 
                            ? null 
                            : new GeoJsonPoint<GeoJson2DGeographicCoordinates>(new GeoJson2DGeographicCoordinates(source.Location.Longitude, source.Location.Latitude)),
                Name = source.Name,
                Sex = source.Sex.GetValueOrDefault(),
                BirthDate = source.BirthDate,
                Styles = source.Styles,
                VideoInfos = source.VideoInfos?.Select(x=>x.ToEntity()).ToList(),
                SoundCloudTracks = source.SoundCloudTracks?.Select(x=>x.ToEntity()).ToList(),
                YouTubeLink = source.YoutubeLink,
                VkLink = source.VkLink,
                FacebookLink = source.FacebookLink,
                Instrument = source.Instrument,
                ExpirienceType = source.Expirience,
                SoundCloudLink = source.SoundCloudLink,
                Address = source.Address,
                IsActive = source.IsActive,
                UserLogin = source.UserLogin
            };
        }
    }
}
