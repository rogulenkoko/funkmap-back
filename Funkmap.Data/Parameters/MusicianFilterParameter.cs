using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public List<ExpirienceType> Expirience { get; set; }
        public List<Styles> Styles { get; set; }
        public EntityType EntityType => EntityType.Musician;

        public override string ToString()
        {
            var separator = ",";
            var sb = new StringBuilder();
            if (Instruments != null) sb.Append($"{String.Join(separator, Instruments)}|");
            if (Expirience != null) sb.Append($"{String.Join(separator, Expirience)}|");
            if (Styles != null) sb.Append($"{String.Join(separator, Styles)}");

            return sb.ToString();
        }
    }

    public class BandFilterParameter : IFilterParameter
    {
        public List<Styles> Styles { get; set; }
        public EntityType EntityType => EntityType.Band;

        public override string ToString()
        {
            var separator = ",";
            return $"{String.Join(separator, Styles)}|";
        }
    }
}
