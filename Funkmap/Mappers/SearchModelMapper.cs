using Funkmap.Data.Entities;
using Funkmap.Data.Entities.Abstract;
using Funkmap.Models;

namespace Funkmap.Mappers
{
    public static class SearchModelMapper
    {
        public static SearchModel ToSearchModel(this BaseEntity source)
        {
            if (source == null) return null;
            return new SearchModel()
            {
                Avatar = source.Photo?.AsByteArray,
                Login = source.Login,
                Title = source.Name,
                Longitude = source.Location.Coordinates.Longitude,
                Latitude = source.Location.Coordinates.Latitude,
                Type = source.EntityType,
                Instrument = (source as MusicianEntity)?.Instrument ?? InstrumentType.None
            };
        }
    }
}
