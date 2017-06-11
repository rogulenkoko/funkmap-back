using System.Linq;
using Funkmap.Module.Musician.Extensions;
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
                DesiredInstruments = source.DesiredInstruments.ToArray(),
                Musicians = source.Musicians.Select(x=>x.ToMusicianPreview()).ToList()
            };
        }
    }
}
