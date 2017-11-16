
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
            var firstCurrent = this;
            var secondCurrent = second as InBandStatisticsEntity;
            if (firstCurrent == null || secondCurrent == null) throw new InvalidOperationException("invalid parameter types");

            var newStatisticsDictionary = secondCurrent.CountStatistics.ToDictionary(x => x.Key);
            foreach (var countStatistic in firstCurrent.CountStatistics)
            {
                if (newStatisticsDictionary.ContainsKey(countStatistic.Key))
                {
                    countStatistic.Count += newStatisticsDictionary[countStatistic.Key].Count;
                }
            }
            return firstCurrent;
        }
    }
}
