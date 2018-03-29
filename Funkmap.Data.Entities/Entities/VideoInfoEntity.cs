using System;
using Funkmap.Domain.Enums;
using MongoDB.Bson.Serialization.Attributes;

namespace Funkmap.Data.Entities.Entities
{
    public class VideoInfoEntity
    {
        [BsonElement("vid")]
        public string Id { get; set; }

        [BsonElement("vn")]
        public string Name { get; set; }

        [BsonElement("vd")]
        public string Description { get; set; }

        [BsonElement("vt")]
        public VideoType Type { get; set; }

        [BsonElement("vsd")]
        public DateTime SaveDateUtc { get; set; }

    }
}
