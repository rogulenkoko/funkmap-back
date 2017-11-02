using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Funkmap.Statistics.Data.Entities
{
    [BsonDiscriminator(RootClass = true)]

    [BsonKnownTypes(typeof(EntityTypeStatisticsEntity))]
    public abstract class BaseStatisticsEntity
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("d")]
        public DateTime LastUpdate { get; set; }

        [BsonElement("t")]
        public StatisticsType StatisticsType { get; set; }
    }
}
