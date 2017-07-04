using Funkmap.Common;
using Funkmap.Data.Entities.Abstract;
using MongoDB.Bson.Serialization.Attributes;

namespace Funkmap.Data.Entities
{
    public class ShopEntity : EstablishmentEntity
    {
        public ShopEntity()
        {
            EntityType = EntityType.Shop;
        }

        [BsonElement("ws")]
        [BsonIgnoreIfDefault]
        public string Website { get; set; }
        
    }
}
