using Funkmap.Domain;

namespace Funkmap.Statistics.Models
{
    public class ProfileTypeStatistics : IStatistics
    {
        public EntityType EntityType { get; set; }
        public int Count { get; set; }
    }
}
