
using System.Collections.Generic;

namespace Funkmap.Statistics.Data.Entities
{
    public class InBandStatisticsEntity : BaseStatisticsEntity
    {
        public List<CountStatisticsEntity<bool>> CountStatistics { get; set; }

        public override BaseStatisticsEntity Merge(BaseStatisticsEntity second)
        {
            //todo
            return second;
        }
    }
}
