using System.Collections.Generic;

namespace Funkmap.Statistics.Models
{
    public class ProfileStatistics
    {
        public ICollection<ProfileTypeStatistics> ProfileTypeStatistics { get; set; }
        public ICollection<CityStatistics> CityStatistics { get; set; }
        public ICollection<TopProfileStatistics> TopProfileStatistics { get; set; }
    }
}
