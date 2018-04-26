using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Funkmap.Common.Data.Mongo.Entities
{
    public class ImageInfoEntity
    {
        [BsonElement("ab")]
        public BsonBinaryData Image { get; set; }
    }

}
