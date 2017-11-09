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
            //тут неуместен мерж
            return second;
        }
    }
}
