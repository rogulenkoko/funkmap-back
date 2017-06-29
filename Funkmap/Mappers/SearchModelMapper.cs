using Funkmap.Data.Entities;
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
                Type = source.EntityType,
                Instrument = (source as MusicianEntity)?.Instrument ?? InstrumentType.None
            };
        }
    }
}
