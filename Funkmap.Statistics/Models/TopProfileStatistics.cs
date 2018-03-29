using Funkmap.Domain;

namespace Funkmap.Statistics.Models
{
    public class TopProfileStatistics : IStatistics
    {
        public string Login { get; set; }
        public EntityType EntityType { get; set; }
        public int Count { get; set; }
    }
}
