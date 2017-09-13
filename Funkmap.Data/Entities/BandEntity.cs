using System.Collections.Generic;
using Funkmap.Common;
using Funkmap.Data.Entities.Abstract;
using MongoDB.Bson.Serialization.Attributes;

namespace Funkmap.Data.Entities
{
    public class BandEntity : BaseEntity
    {
        public BandEntity()
        {
            EntityType = EntityType.Band;
            Styles = new List<Styles>();
            MusicianLogins = new List<string>();
            InvitedMusicians = new List<string>();
        }

        [BsonElement("dinstr")]
        [BsonIgnoreIfDefault]
        public List<InstrumentType> DesiredInstruments { get; set; }

        [BsonElement("vl")]
        [BsonIgnoreIfDefault]
        public List<string> VideoLinks { get; set; }

        [BsonElement("mus")]
        [BsonIgnoreIfDefault]
        public List<string> MusicianLogins{ get; set; }

        [BsonElement("stls")]
        [BsonIgnoreIfDefault]
        public List<Styles> Styles { get; set; }

        public List<string> InvitedMusicians { get; set; }
    }
}
