using System.Collections.Generic;
using System.Linq;
using Funkmap.Data.Entities.Entities;
using Funkmap.Data.Entities.Entities.Abstract;
using Funkmap.Domain.Enums;
using Funkmap.Domain.Models;

namespace Funkmap.Data.Mappers
{
    public static class MarkerModelMapper
    {
        public static List<Marker> ToMarkers(this IReadOnlyCollection<BaseEntity> source)
        {
            return source?.Select(x => x.ToMarker()).ToList();
        }

        public static Marker ToMarker(this BaseEntity source)
        {
            if (source == null) return null;
            return new Marker
            {
                Login = source.Login,
                Longitude = source.Location.Coordinates.Longitude,
                Latitude = source.Location.Coordinates.Latitude,
                ModelType = source.EntityType,
                Instrument = (source as MusicianEntity)?.Instrument ?? Instruments.None
            };
        }
    }
}
