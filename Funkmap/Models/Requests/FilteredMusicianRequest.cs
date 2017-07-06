using System.Collections.Generic;
using Funkmap.Data.Entities;

namespace Funkmap.Models.Requests
{
    public class FilteredMusicianRequest
    {
        public InstrumentType Instrument { get; set; }
        public ExpirienceType Expirience { get; set; }
        public List<Styles> Styles { get; set; }
    }
}
