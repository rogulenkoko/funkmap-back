using System;
using System.Collections.Generic;
using System.Linq;
using Funkmap.Common;
using MongoDB.Bson.Serialization.Attributes;

namespace Funkmap.Statistics.Data.Entities
{
    public class EntityTypeStatisticsEntity : BaseStatisticsEntity
    {
        public EntityTypeStatisticsEntity()
        {
            CountStatistics = new List<CountStatisticsEntity<EntityType>>();
            StatisticsType = StatisticsType.EntityType;
        }

        [BsonElement("cs")]
        public List<CountStatisticsEntity<EntityType>> CountStatistics { get; set; }

        public override BaseStatisticsEntity Merge(BaseStatisticsEntity second)
        {
            var firstCurrent = this;
            var secondCurrent = second as EntityTypeStatisticsEntity;
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
