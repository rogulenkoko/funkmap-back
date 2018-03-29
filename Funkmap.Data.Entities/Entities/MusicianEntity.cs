using System;
using System.Collections.Generic;
using Funkmap.Data.Entities.Entities.Abstract;
using Funkmap.Domain.Enums;
using MongoDB.Bson.Serialization.Attributes;
using Funkmap.Domain;

namespace Funkmap.Data.Entities.Entities
{
    public class MusicianEntity : BaseEntity
    {
        public MusicianEntity()
        {
            EntityType = EntityType.Musician;
            Sex = Sex.None;
            Instrument = Instruments.None;
        }

        [BsonElement("intsr")]
        public Instruments Instrument { get; set; }

        [BsonElement("sex")]
        public Sex Sex { get; set; }

        [BsonElement("bd")]
        [BsonIgnoreIfDefault]
        public DateTime? BirthDate { get; set; }

        [BsonElement("stls")]
        [BsonIgnoreIfDefault]
        public List<Styles> Styles { get; set; }

        [BsonElement("exp")]
        public Expiriences ExpirienceType { get; set; }

        [BsonElement("band")]
        [BsonIgnoreIfDefault]
        public List<string> BandLogins { get; set; }
    }
}
