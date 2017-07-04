using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Funkmap.Data.Entities.Abstract
{
    public abstract class EstablishmentEntity : BaseEntity
    {
        [BsonElement("addr")]
        [BsonIgnoreIfDefault]
        public string Address { get; set; }

        [BsonElement("workh")]
        [BsonIgnoreIfDefault]
        public string WorkingHoursDescription { get; set; }

        [BsonElement("price")]
        [BsonIgnoreIfDefault]
        public BsonBinaryData PriceListFile { get; set; }

        //todo public List<string> Comments { get; set; }
    }
}
