using System.Collections.Generic;
using Funkmap.Common;
using Funkmap.Data.Entities;

namespace Funkmap.Data.Parameters
{
    public interface IFilterParameter
    {
        EntityType EntityType { get; }
    }
    public class MusicianFilterParameter : IFilterParameter
    {
        public List<InstrumentType> Instruments { get; set; }
        public ExpirienceType Expirience { get; set; }
        public List<Styles> Styles { get; set; }
        public EntityType EntityType => EntityType.Musician;
    }

    public class BandFilterParameter : IFilterParameter
    {
        public List<Styles> Styles { get; set; }
        public EntityType EntityType => EntityType.Band;
    }
}
