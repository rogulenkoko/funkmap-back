using System;
using System.Collections.Generic;
using Funkmap.Data.Entities.Entities.Abstract;
using MongoDB.Bson.Serialization.Attributes;

namespace Funkmap.Data.Entities.Entities
{
    public class MusicianEntity : BaseEntity
    {
        public MusicianEntity()
        {
            EntityType = EntityType.Musician;
            Sex = Sex.None;
            Instrument = InstrumentType.None;
        }

        [BsonElement("intsr")]
        
        public InstrumentType Instrument { get; set; }

        [BsonElement("sex")]
        public Sex Sex { get; set; }

        [BsonElement("bd")]
        [BsonIgnoreIfDefault]
        public DateTime? BirthDate { get; set; }

        [BsonElement("stls")]
        [BsonIgnoreIfDefault]
        public List<Styles> Styles { get; set; }

        [BsonElement("exp")]
        public ExpirienceType ExpirienceType { get; set; }

        [BsonElement("band")]
        [BsonIgnoreIfDefault]
        public List<string> BandLogins { get; set; }
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
        Funk = 3,
        Metal = 4,
        Jazz = 5,
        Pop = 6,
        Electronic = 7
    }

    public enum InstrumentType
    {
        None = 0,
        Bass = 1,
        Drums = 2,
        Vocal = 3,
        Brass = 4,
        Guitar = 5,
        Keyboard = 6,
        Dj = 7
    }

    public enum ExpirienceType
    {
        None = 0,
        Begginer = 1,
        Middle = 2,
        Advanced = 3,
        SuperStar = 4
    }
}
