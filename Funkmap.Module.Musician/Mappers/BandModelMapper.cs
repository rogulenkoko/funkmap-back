using System;
using System.Linq;
using Funkmap.Module.Musician.Models;
using Funkmap.Musician.Data.Entities;

namespace Funkmap.Module.Musician.Mappers
{
    public static class BandModelMapper
    {
        public static BandModel ToModel(this BandEntity source)
        {
            if (source == null) return null;
            return new BandModel()
            {
                Login = source.Login,
                Longitude = source.Longitude,
                Latitude = source.Latitude,
                Name = source.Name,
                ShowPrice = source.ShowPrice,
                VideoLinks = source.VideoLinks.ToList(),
                DesiredInstruments = Enum.GetValues(typeof(InstrumentType))
                    .Cast<InstrumentType>()
                    .Where(allInstruments => (source.DesiredInstruments & allInstruments) != 0)
                    .ToArray(),
                Musicians = source.Musicians.Select(x=>x.ToMusicianPreview()).ToList()
            };
        }
    }
}
