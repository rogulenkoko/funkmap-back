using Funkmap.Domain.Enums;

namespace Funkmap.Statistics.Models
{
    public class SexStatistics : IStatistics
    {
        public Sex Sex { get; set; }
        public int Count { get; set; }
    }
}
