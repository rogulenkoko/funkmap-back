using Funkmap.Data.Entities.Entities;

namespace Funkmap.Statistics.Models
{
    public class SexStatistics : IStatistics
    {
        public Sex Sex { get; set; }
        public int Count { get; set; }
    }
}
