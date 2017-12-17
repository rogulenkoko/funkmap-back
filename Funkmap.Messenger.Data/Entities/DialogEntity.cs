using System;
using System.Collections.Generic;
using Funkmap.Common.Data.Mongo.Abstract;
using Funkmap.Common.Data.Mongo.Entities;
using MongoDB.Bson.Serialization.Attributes;

namespace Funkmap.Messenger.Data.Entities
{
    public class DialogEntity : MongoEntity
    {

        [BsonElement("n")]
        [BsonIgnoreIfDefault]
        public string Name { get; set; }

        [BsonIgnore]
        public ImageInfo Avatar { get; set; }

        [BsonElement("prtcpnts")]
        public List<string> Participants { get; set; }

        [BsonElement("lmd")]
        public DateTime LastMessageDate { get; set; }

        [BsonElement("c")]
        [BsonIgnoreIfDefault]
        public string CreatorLogin { get; set; }

        [BsonIgnore]
        public MessageEntity LastMessage { get; set; }

    }
}
