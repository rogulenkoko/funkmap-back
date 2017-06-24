using System;
using System.Linq;
using Funkmap.Data.Entities;
using Funkmap.Models;
using MongoDB.Driver.GeoJsonObjectModel;

namespace Funkmap.Mappers
{
    public static class MusicianModelMapper
    {
        public static MusicianModel ToMusicianModel(this MusicianEntity source)
        {
            if (source == null) return null;
            return new MusicianModel()
            {
                Login = source.Login,
                Description = source.Description,
                Latitude = source.Location.Coordinates.Latitude,
                Longitude = source.Location.Coordinates.Longitude,
                Name = source.Name,
                Sex = source.Sex,
                BirthDate = source.BirthDate,
                Age = (int)Math.Round((DateTime.Now - source.BirthDate).TotalDays / 365),
                Styles = source.Styles.ToArray(),
                Avatar = source.Photo?.Bytes,
                YouTubeLink = source.YouTubeLink,
                VkLink = source.VkLink,
                FacebookLink = source.FacebookLink,
                Instrument = source.Instrument
            };
        }

        public static MusicianEntity ToMusicianEntity(this MusicianModel source)
        {
            if (source == null) return null;
            
            return new MusicianEntity()
            {
                Login = source.Login,
                Description = source.Description,
                Location = new GeoJsonPoint<GeoJson2DGeographicCoordinates>(new GeoJson2DGeographicCoordinates(source.Longitude, source.Latitude)),
                Name = source.Name,
                Sex = source.Sex,
                BirthDate = source.BirthDate,
                Styles = source.Styles.ToList(),
                Photo = source.Avatar ?? new byte[] {},
                YouTubeLink = source.YouTubeLink,
                VkLink = source.VkLink,
                FacebookLink = source.FacebookLink,
                Instrument = source.Instrument
            };
        }
    }
}
