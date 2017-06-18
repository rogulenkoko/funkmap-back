using System.Linq;
using Funkmap.Data.Entities;
using Funkmap.Models;

namespace Funkmap.Mappers
{
    public static class BandModelMapper
    {
        public static BandModel ToModel(this BandEntity source)
        {
            if (source == null) return null;
            return new BandModel()
            {
                Login = source.Login,
                Longitude = source.Location.Coordinates.Longitude,
                Latitude = source.Location.Coordinates.Latitude,
                Name = source.Name,
                ShowPrice = source.ShowPrice,
                VideoLinks = source.VideoLinks?.ToList(),
                DesiredInstruments = source.DesiredInstruments?.ToList(),
                //todo
                //Musicians = source.Musicians.Select(x=>x.ToMusicianPreview()).ToList()
            };
        }
    }
}
