using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace Funkmap.Statistics.Data.Entities
{
    public class AgeStatisticsEntity : BaseStatisticsEntity
    {
        public AgeStatisticsEntity()
        {
            CountStatistics = new List<CountStatisticsEntity<string>>();
            StatisticsType = StatisticsType.Age;
        }

        [BsonElement("cs")]
        public List<CountStatisticsEntity<string>> CountStatistics { get; set; }

        public override BaseStatisticsEntity Merge(BaseStatisticsEntity second)
        {
            return second;
        }
    }
}

