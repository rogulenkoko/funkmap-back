using System.Collections.Generic;

namespace Funkmap.Statistics.Models
{
    public class MusicianStatistics
    {
        public ICollection<InstrumentStatistics> InstrumentStatistics { get; set; }
        public ICollection<SexStatistics> SexStatistics { get; set; }
        public ICollection<AgeStatistics> AgeStatistics { get; set; }
        public ICollection<InBandStatistics> BandStatistics { get; set; }
        public ICollection<StyleStatistics> StyleStatistics { get; set; }
    }
}
