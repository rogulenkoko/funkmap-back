using Funkmap.Domain;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Funkmap.Statistics.Data.Objects
{
    public class TopEntityStatistic
    {
        [BsonId]
        public string Login { get; set; }

        [BsonElement("t")]
        public EntityType EntityType { get; set; }

        [BsonElement("count")]
        public int Count { get; set; }
    }
}
