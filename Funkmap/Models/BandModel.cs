using System.Collections.Generic;
using Funkmap.Data.Entities.Entities;

namespace Funkmap.Models
{
    public class BandModel : BaseModel
    {
        public List<Styles> Styles { get; set; }
        public List<InstrumentType> DesiredInstruments { get; set; }
        public List<string> Musicians { get; set; }
    }

    public class BandModelPreview : BaseModel
    {

        public List<Styles> Styles { get; set; }

        public List<InstrumentType> DesiredInstruments { get; set; }
    }
}
