using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Common.Data.Tools;
using Funkmap.Musician.Data.Entities;

namespace Funkmap.Module.Musician.Models
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
        public ICollection<MusicianPreview> Musicians { get; set; }
    }
}
