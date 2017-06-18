using Funkmap.Data.Entities.Abstract;
using MongoDB.Bson.Serialization.Attributes;

namespace Funkmap.Data.Entities
{
    public class ShopEntity : BaseEntity
    {
        [BsonElement("ws")]
        [BsonIgnoreIfDefault]
        public string WebSite { get; set; }
    }
}
