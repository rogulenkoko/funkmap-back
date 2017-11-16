
using System;
using System.Collections.Generic;
using System.Linq;

namespace Funkmap.Statistics.Data.Entities
{
    public class InBandStatisticsEntity : BaseStatisticsEntity
    {
        public InBandStatisticsEntity()
        {
            StatisticsType = StatisticsType.InBand;
        }

        public List<CountStatisticsEntity<bool>> CountStatistics { get; set; }

        public override BaseStatisticsEntity Merge(BaseStatisticsEntity second)
        {
            return second;
        }
    }
}
