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

        [BsonElement("sex")]
        [BsonIgnoreIfDefault]
        public Sex Sex { get; set; }

        [BsonElement("bd")]
        [BsonIgnoreIfDefault]
        public DateTime BirthDate { get; set; }

        [BsonElement("stls")]
        [BsonIgnoreIfDefault]
        public List<Styles> Styles { get; set; }

        [BsonElement("band")]
        [BsonIgnoreIfDefault]
        public ObjectId BandId { get; set; }

        [BsonElement("bandlog")]
        [BsonIgnoreIfDefault]
        public string BandLogin { get; set; }
    }

    public enum Sex
    {
        Male = 1,
        Female = 2
    }

    public enum Styles
    {
        HipHop = 1,
        Rock = 2,
        Funk = 3
    }

    public enum InstrumentType
    {
        Bass = 1,
        Drums = 2,
        Vocal = 3,
        Brass = 4,
        Guitar = 5,
        Keyboard = 6
    }
}
