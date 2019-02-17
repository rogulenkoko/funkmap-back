using System;
using System.Collections.Generic;
using System.Text;
using Funkmap.Domain.Abstract;
using Funkmap.Domain.Enums;

namespace Funkmap.Domain.Parameters
{
    public class MusicianFilterParameter : IFilterParameter
    {
        public List<Instruments> Instruments { get; set; }
        public List<Experiences> Experiences { get; set; }
        public List<Styles> Styles { get; set; }
        public EntityType EntityType => EntityType.Musician;

        public override string ToString()
        {
            var separator = ",";
            var sb = new StringBuilder();
            if (Instruments != null) sb.Append($"{String.Join(separator, Instruments)}|");
            if (Experiences != null) sb.Append($"{String.Join(separator, Experiences)}|");
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
