using Funkmap.Domain.Enums;

namespace Funkmap.Statistics.Models
{
    public class StyleStatistics : IStatistics
    {
        public Styles Style { get; set; }
        public int Count { get; set; }
    }
}
