
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Funkmap.Auth.Data.Entities
{
    public class UserEntity
    {
        public UserEntity()
        {
            Favourites = new List<string>();
        }

        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("log")]
        public string Login { get; set; }

        [BsonElement("pass")]
        public string Password { get; set; }

        [BsonElement("em")]
        public string Email { get; set; }

        [BsonElement("av")]
        [BsonIgnoreIfDefault]
        public BsonBinaryData Avatar { get; set; }

        [BsonElement("favs")]
        [BsonIgnoreIfDefault]
        public List<string> Favourites { get; set; }
    }
}
