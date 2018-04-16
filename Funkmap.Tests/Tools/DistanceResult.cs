using System.Collections.Generic;
using Funkmap.Data.Entities.Entities.Abstract;
using MongoDB.Bson.Serialization.Attributes;

namespace Funkmap.Tests.Tools
{
    internal class DistanceResult
    {
        [BsonElement("results")]
        public List<DistanceBaseEntity> Results { get; set; }

        [BsonElement("stats")]
        public Statistics Statistics { get; set; }

        [BsonElement("ok")]
        public int Ok { get; set; }
    }

    internal class DistanceBaseEntity
    {
        [BsonElement("dis")]
        public double Distance { get; set; }

        [BsonElement("obj")]
        public BaseEntity Profile { get; set; }
    }

    internal class Statistics
    {
        [BsonElement("nscanned")]
        public double NScanned { get; set; }

        [BsonElement("objectsLoaded")]
        public double ObjectsLoaded { get; set; }

        [BsonElement("avgDistance")]
        public double AvgDistance { get; set; }

        [BsonElement("maxDistance")]
        public double MaxDistance { get; set; }

        [BsonElement("time")]
        public double Time { get; set; }
    }
}
