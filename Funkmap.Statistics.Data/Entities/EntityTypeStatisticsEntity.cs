using System.Collections.Generic;
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
    }
}
