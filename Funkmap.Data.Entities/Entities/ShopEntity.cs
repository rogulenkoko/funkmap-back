using Funkmap.Data.Entities.Entities.Abstract;
using MongoDB.Bson.Serialization.Attributes;

namespace Funkmap.Data.Entities.Entities
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
