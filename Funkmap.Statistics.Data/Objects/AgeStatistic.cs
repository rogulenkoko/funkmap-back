using MongoDB.Bson.Serialization.Attributes;

namespace Funkmap.Statistics.Data.Objects
{
    public class AgeStatistic
    {
        [BsonId]
        public string Desc { get; set; }

        [BsonElement("value")]
        public int Count { get; set; }
    }
}
