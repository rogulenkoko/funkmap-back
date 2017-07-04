using System;
using System.Collections.Generic;
using Funkmap.Common;
using Funkmap.Data.Entities.Abstract;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Funkmap.Data.Entities
{
    public class MusicianEntity : BaseEntity
    {
        public MusicianEntity()
        {
            EntityType = EntityType.Musician;
        }

        [BsonElement("intsr")]
        public InstrumentType Instrument { get; set; }

        [BsonElement("sex")]
        [BsonIgnoreIfDefault]
        public Sex Sex { get; set; }

        [BsonElement("bd")]
        [BsonIgnoreIfDefault]
        public DateTime BirthDate { get; set; }

        [BsonElement("stls")]
        [BsonIgnoreIfDefault]
        public List<Styles> Styles { get; set; }

        [BsonElement("exp")]
        public ExpirienceType ExpirienceType { get; set; }

        [BsonElement("band")]
        [BsonIgnoreIfDefault]
        public ObjectId BandId { get; set; }

        [BsonElement("bandlog")]
        [BsonIgnoreIfDefault]
        public string BandLogin { get; set; }
    }

    public enum Sex
    {
        None = 0,
        Male = 1,
        Female = 2
    }

    public enum Styles
    {
        None = 0,
        HipHop = 1,
        Rock = 2,
        Funk = 3
    }

    public enum InstrumentType
    {
        None = 0,
        Bass = 1,
        Drums = 2,
        Vocal = 3,
        Brass = 4,
        Guitar = 5,
        Keyboard = 6
    }

    public enum ExpirienceType
    {
        Begginer = 1,
        Middle = 2,
        Advanced = 3,
        SuperStar = 4
    }
}
