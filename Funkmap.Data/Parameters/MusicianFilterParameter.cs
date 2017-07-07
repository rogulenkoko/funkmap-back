using System.Collections.Generic;
using Funkmap.Data.Entities;

namespace Funkmap.Data.Parameters
{
    public class MusicianFilterParameter
    {
        public List<InstrumentType> Instruments { get; set; }
        public ExpirienceType Expirience { get; set; }
        public List<Styles> Styles { get; set; }
    }
}
