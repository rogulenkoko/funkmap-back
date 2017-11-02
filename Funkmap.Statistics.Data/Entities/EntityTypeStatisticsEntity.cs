using System.Collections.Generic;
using Funkmap.Common;

namespace Funkmap.Statistics.Data.Entities
{
    public class EntityTypeStatisticsEntity : BaseStatisticsEntity
    {
        public EntityTypeStatisticsEntity()
        {
            CountStatistics = new List<CountStatisticsEntity<EntityType>>();
            StatisticsType = StatisticsType.EntityType;
        }
        public List<CountStatisticsEntity<EntityType>> CountStatistics { get; set; }
    }
}
