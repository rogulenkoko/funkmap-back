using System;
using System.Linq;
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
                Styles = Enum.GetValues(typeof(Styles)).Cast<Styles>().Where(allStyles => (source.Styles & allStyles) != 0).ToArray()

            };
        }
    }
}
