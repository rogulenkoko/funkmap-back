using Funkmap.Data.Entities;

namespace Funkmap.Statistics.Models
{
   public class InstrumentStatistics : IStatistics
    {
        public InstrumentType Instrument { get; set; }

        public int Count { get; set; }
    }
}
