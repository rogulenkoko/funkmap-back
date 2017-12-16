using Funkmap.Data.Entities;
using Funkmap.Data.Entities.Entities;

namespace Funkmap.Statistics.Models
{
   public class InstrumentStatistics : IStatistics
    {
        public InstrumentType Instrument { get; set; }

        public int Count { get; set; }
    }
}
