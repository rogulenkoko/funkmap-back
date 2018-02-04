using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Funkmap.Concerts.Entities
{
    public class ConcertEntity
    {
        public ConcertEntity()
        {
            Participants = new List<string>();
            IsFinished = false;
        }

        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("cd")]
        public DateTime CreationDateUtc { get; set; }

        [BsonElement("pb")]
        public DateTime PeriodBeginUtc { get; set; }

        [BsonElement("pe")]
        public DateTime PeriodEndUtc { get; set; }


        [BsonElement("d")]
        public DateTime DateUtc { get; set; }

        [BsonElement("clat")]
        public double Latitude { get; set; }

        [BsonElement("clon")]
        public double Longitude { get; set; }

        [BsonElement("n")]
        public string Name { get; set; }

        [BsonElement("d")]
        public string Description { get; set; }

        [BsonElement("au")]
        public string AfficheUrl { get; set; }

        [BsonElement("cl")]
        public string CreatorLogin { get; set; }

        [BsonElement("ac")]
        public bool IsActive { get; set; }
        
        [BsonElement("f")]
        public bool IsFinished { get; set; }

        /// <summary>
        /// Логины профилей, которые принимают участие к концерту
        /// </summary>
        [BsonElement("ptcp")]
        public List<string> Participants { get; set; }
    }
}
