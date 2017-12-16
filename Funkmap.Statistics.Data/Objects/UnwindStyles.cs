using Funkmap.Data.Entities.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Funkmap.Statistics.Data.Objects
{
    public class UnwindStyles
    {

        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("stls")]
        public Styles Style { get; set; }
    }
}
