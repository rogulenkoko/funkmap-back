using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Funkmap.Data.Entities.Entities.Abstract
{
    public abstract class EstablishmentEntity : BaseEntity
    {
        [BsonElement("workh")]
        [BsonIgnoreIfDefault]
        public string WorkingHoursDescription { get; set; }

        [BsonElement("price")]
        [BsonIgnoreIfDefault]
        public BsonBinaryData PriceListFile { get; set; }

        //todo public List<string> Comments { get; set; }
    }
}
