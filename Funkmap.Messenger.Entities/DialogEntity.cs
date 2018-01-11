﻿using System;
using System.Collections.Generic;
using Funkmap.Common.Data.Mongo.Abstract;
using Funkmap.Common.Data.Mongo.Entities;
using MongoDB.Bson.Serialization.Attributes;

namespace Funkmap.Messenger.Entities
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
        [BsonIgnoreIfDefault]
        public DateTime LastMessageDate { get; set; }

        [BsonElement("c")]
        public string CreatorLogin { get; set; }

        [BsonIgnore]
        public MessageEntity LastMessage { get; set; }

    }
}