using Funkmap.Domain.Enums;

namespace Funkmap.Statistics.Models
{
   public class InstrumentStatistics : IStatistics
    {
        public Instruments Instrument { get; set; }

        public int Count { get; set; }
    }
}
