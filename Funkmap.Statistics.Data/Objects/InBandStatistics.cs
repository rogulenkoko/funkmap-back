using MongoDB.Bson.Serialization.Attributes;

namespace Funkmap.Statistics.Data.Objects
{
    public class InBandStatistics
    {
        [BsonId]
        public bool IsInBand { get; set; }

        [BsonElement("value")]
        public int Count { get; set; }
    }
}
