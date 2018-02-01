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
        }

        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("pb")]
        public DateTime PeriodBegin { get; set; }

        [BsonElement("pe")]
        public DateTime PeriodEnd { get; set; }


        [BsonElement("d")]
        public DateTime Date { get; set; }

        [BsonElement("n")]
        public string Name { get; set; }

        [BsonElement("d")]
        public string Description { get; set; }

        [BsonElement("au")]
        public string AfficheUrl { get; set; }

        [BsonElement("cl")]
        public string CreatorLogin { get; set; }

        /// <summary>
        /// Логины профилей, которые принимают участие к концерту
        /// </summary>
        public List<string> Participants { get; set; }
    }
}
