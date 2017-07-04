using Funkmap.Data.Entities;
using Funkmap.Data.Entities.Abstract;
using Funkmap.Models;

namespace Funkmap.Mappers
{
    public static class MarkerModelMapper
    {
        public static MarkerModel ToMarkerModel(this BaseEntity source)
        {
            if (source == null) return null;
            return new MarkerModel()
            {
                Login = source.Login,
                Longitude = source.Location.Coordinates.Longitude,
                Latitude = source.Location.Coordinates.Latitude,
                ModelType = source.EntityType,
                Instrument = (source as MusicianEntity)?.Instrument ?? InstrumentType.None
            };
        }
    }
}
