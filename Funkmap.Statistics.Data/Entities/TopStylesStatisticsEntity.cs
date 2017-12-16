using System.Collections.Generic;
using Funkmap.Data.Entities.Entities;
using MongoDB.Bson.Serialization.Attributes;

namespace Funkmap.Statistics.Data.Entities
{
    public class TopStylesStatisticsEntity : BaseStatisticsEntity
    {
        public TopStylesStatisticsEntity()
        {
            CountStatistics = new List<CountStatisticsEntity<Styles>>();
            StatisticsType = StatisticsType.TopStyles;
        }

        [BsonElement("cs")]
        public List<CountStatisticsEntity<Styles>> CountStatistics { get; set; }

        public override BaseStatisticsEntity Merge(BaseStatisticsEntity second)
        {
            return second;
        }
    }
}

