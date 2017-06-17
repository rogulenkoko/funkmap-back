using System;
using System.Linq;
using Funkmap.Module.Musician.Extensions;
using Funkmap.Module.Musician.Models;
using Funkmap.Musician.Data.Entities;

namespace Funkmap.Module.Musician.Mappers
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
                Expirience = source.Expirience,
                Latitude = source.Latitude,
                Longitude = source.Longitude,
                Name = source.Name,
                Sex = source.Sex,
                BirthDate = source.BirthDate,
                Age = (int)Math.Round((DateTime.Now - source.BirthDate).TotalDays / 365),
                Styles = source.Styles.ToArray(),
                Avatar = source.AvatarImage,
                YouTubeLink = source.YouTubeLink,
                VkLink = source.VkLink,
                FacebookLink = source.FacebookLink,
                Instrument = source.Instrument
            };
        }

        public static MusicianEntity ToMusicianEntity(this MusicianModel source)
        {
            if (source == null) return null;
            Styles styleFilter = Styles.None;
            if (source.Styles != null)
            {
                styleFilter = source.Styles.FirstOrDefault();
                if (source.Styles.Length > 1)
                {
                    for (int i = 1; i < source.Styles.Length; i++)
                    {
                        styleFilter = styleFilter | source.Styles.ElementAt(i);
                    }
                }
            }
            
            return new MusicianEntity()
            {
                Login = source.Login,
                Description = source.Description,
                Expirience = source.Expirience,
                Latitude = source.Latitude,
                Longitude = source.Longitude,
                Name = source.Name,
                Sex = source.Sex,
                BirthDate = source.BirthDate,
                Styles = styleFilter,
                AvatarImage = source.Avatar,
                YouTubeLink = source.YouTubeLink,
                VkLink = source.VkLink,
                FacebookLink = source.FacebookLink,
                Instrument = source.Instrument
            };
        }

        public static MusicianPreview ToMusicianPreview(this MusicianEntity source)
        {
            if (source == null) return null;
            return new MusicianPreview()
            {
                Name = source.Name,
                Id = source.Id
            };
        }
    }
}
