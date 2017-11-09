using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Common;
using Funkmap.Data.Entities.Abstract;
using Funkmap.Statistics.Data.Objects;
using MongoDB.Bson.Serialization.Attributes;

namespace Funkmap.Statistics.Data.Entities
{
    public class TopEntityStatisticsEntity : BaseStatisticsEntity
    {
        public TopEntityStatisticsEntity()
        {
            CountStatistics = new List<TopEntityStatistic>();
            StatisticsType = StatisticsType.TopEntity;
        }

        [BsonElement("cs")]
        public List<TopEntityStatistic> CountStatistics { get; set; }

        public override BaseStatisticsEntity Merge(BaseStatisticsEntity second)
        {
            var firstCurrent = this;
            var secondCurrent = second as TopEntityStatisticsEntity;
            if (firstCurrent == null || secondCurrent == null) throw new InvalidOperationException("invalid parameter types");

            //var newStatisticsDictionary = secondCurrent.CountStatistics.ToDictionary(x => x.Key);
            //foreach (var countStatistic in firstCurrent.CountStatistics)
            //{
            //    if (newStatisticsDictionary.ContainsKey(countStatistic.Key))
            //    {
            //        countStatistic.Count += newStatisticsDictionary[countStatistic.Key].Count;
            //    }
            //}
            return null;
        }
    }
}
