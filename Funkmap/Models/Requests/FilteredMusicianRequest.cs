using System.Collections.Generic;
using Funkmap.Data.Entities;

namespace Funkmap.Models.Requests
{
    public class FilteredMusicianRequest
    {
        public string SearchText { get; set; }
        public List<InstrumentType> Instruments { get; set; }
        public ExpirienceType Expirience { get; set; }
        public List<Styles> Styles { get; set; }
    }
}
