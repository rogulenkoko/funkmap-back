using System.Collections.Generic;
using System.Linq;
using Funkmap.Common;
using Funkmap.Data.Entities;
using ServiceStack;

namespace Funkmap.Data.Parameters
{
    public interface IFilterParameter
    {
        EntityType EntityType { get; }
    }
    public class MusicianFilterParameter : IFilterParameter
    {
        public List<InstrumentType> Instruments { get; set; }
        public List<ExpirienceType> Expirience { get; set; }
        public List<Styles> Styles { get; set; }
        public EntityType EntityType => EntityType.Musician;

        public override string ToString()
        {
            return $"{Instruments?.Join(",")}|{Expirience?.Join(",")}|{Styles?.Join(",")}|";
        }
    }

    public class BandFilterParameter : IFilterParameter
    {
        public List<Styles> Styles { get; set; }
        public EntityType EntityType => EntityType.Band;

        public override string ToString()
        {
            return $"{Styles?.Join(",")}|";
        }
    }
}
