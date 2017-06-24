using System.Collections.Generic;
using Funkmap.Data.Entities;

namespace Funkmap.Models
{
    public class BandModel
    {
        public string Name { get; set; }
        public string Login { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double ShowPrice { get; set; }
        public ICollection<InstrumentType> DesiredInstruments { get; set; }
        public ICollection<string> VideoLinks { get; set; }
        public ICollection<string> Musicians { get; set; } //todo на превью
    }
}
