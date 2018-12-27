using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Funkmap.Auth.Data.Entities
{
    internal class UserEntity
    {
        [BsonId]
        public string Login { get; set; }

        [BsonElement("n")]
        public string Name { get; set; }

        [BsonElement("pass")]
        public string Password { get; set; }

        [BsonElement("em")]
        public string Email { get; set; }

        [BsonElement("loc")]
        public string Locale { get; set; }

        [BsonElement("av")]
        [BsonIgnoreIfDefault]
        public string AvatarUrl { get; set; }

        [BsonElement("date")]
        [BsonIgnoreIfDefault]
        public DateTime LastVisitDateUtc { get; set; }

        [BsonElement("soc")]
        [BsonIgnoreIfDefault]
        public bool IsSocial { get; set; }
    }
}
