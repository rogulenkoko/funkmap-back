using System.Collections.Generic;
using Funkmap.Common;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Funkmap.Data.Entities
{
    public class BandEntity : BaseEntity
    {
        public BandEntity()
        {
            EntityType = EntityType.Band;
        }

        [BsonElement("sp")]
        [BsonIgnoreIfDefault]
        public double ShowPrice { get; set; }

        [BsonElement("dinstr")]
        [BsonIgnoreIfDefault]
        public List<InstrumentType> DesiredInstruments { get; set; }

        [BsonElement("vl")]
        [BsonIgnoreIfDefault]
        public List<string> VideoLinks { get; set; }

        [BsonElement("mus")]
        [BsonIgnoreIfDefault]
        public List<string> MusicianLogins{ get; set; }
    }
}
