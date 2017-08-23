using System;
using System.Collections.Generic;
using Funkmap.Common.Data.Mongo.Abstract;
using MongoDB.Bson.Serialization.Attributes;

namespace Funkmap.Messenger.Data.Entities
{
    public class DialogEntity : MongoEntity
    {
        public DialogEntity()
        {
            Participants = new List<string>();
        }

        [BsonElement("prtcpnts")]
        public List<string> Participants { get; set; }

        [BsonElement("lmd")]
        public DateTime LastMessageDate { get; set; }

    }
}
