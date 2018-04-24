using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Funkmap.Common.Data.Mongo.Entities
{
    public class ImageInfo
    {
        [BsonElement("ab")]
        public BsonBinaryData Image { get; set; }
    }

}
