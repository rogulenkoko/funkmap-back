using System.Collections.Generic;
using Funkmap.Data.Entities;

namespace Funkmap.Models
{
    public class BandModel : BaseModel
    {
        public ICollection<Styles> Styles { get; set; }
        public ICollection<InstrumentType> DesiredInstruments { get; set; }
        public ICollection<string> VideoLinks { get; set; }
        public ICollection<string> Musicians { get; set; }
    }

    public class BandModelPreview : BaseModel
    {

        public ICollection<Styles> Styles { get; set; }

        public ICollection<InstrumentType> DesiredInstruments { get; set; }
    }
}
