using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Funkmap.Data.Entities.Entities
{
    public class ProAccountEntity
    {
        [BsonElement("u")]
        public string UserLogin { get; set; }

        [BsonElement("exp")]
        public DateTime ExpireAt { get; set; }
    }
}
