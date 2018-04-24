using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Funkmap.Data.Entities.Entities
{
    public class AudioInfoEntity
    {
        [BsonElement("aid")]
        public long Id { get; set; }

        [BsonElement("asd")]
        public DateTime SaveDateUtc { get; set; }
    }
}
