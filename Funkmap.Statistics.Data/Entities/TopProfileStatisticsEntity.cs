using System.Collections.Generic;
using Funkmap.Statistics.Data.Objects;
using MongoDB.Bson.Serialization.Attributes;

namespace Funkmap.Statistics.Data.Entities
{
    public class TopProfileStatisticsEntity : BaseStatisticsEntity
    {
        public TopProfileStatisticsEntity()
        {
            CountStatistics = new List<TopEntityStatistic>();
            StatisticsType = StatisticsType.TopEntity;
        }

        [BsonElement("cs")]
        public List<TopEntityStatistic> CountStatistics { get; set; }

        public override BaseStatisticsEntity Merge(BaseStatisticsEntity second)
        {
            //тут неуместен мерж
            return second;
        }
    }
}
