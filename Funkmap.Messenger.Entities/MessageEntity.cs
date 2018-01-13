using System;
using System.Collections.Generic;
using Funkmap.Common.Data.Mongo.Abstract;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Funkmap.Messenger.Entities
{
    public class MessageEntity : MongoEntity
    {
        public MessageEntity()
        {
            DateTimeUtc = DateTime.UtcNow;
            Content = new List<ContentItemEntity>();
            ToParticipants = new List<string>();
            MessageType = MessageType.Base;
        }

        [BsonElement("d")]
        public ObjectId DialogId { get; set; }

        [BsonElement("date")]
        public DateTime DateTimeUtc { get; set; }

        [BsonElement("sen")]
        public string Sender { get; set; }
        
        [BsonElement("t")]
        public string Text { get; set; }

        [BsonElement("pwr")]
        public List<string> ToParticipants { get; set; }

        [BsonElement("tp")]
        public MessageType MessageType { get; set; }

        [BsonElement("cont")]
        [BsonIgnoreIfDefault]
        public List<ContentItemEntity> Content { get; set; }

        [BsonElement("ir")]
        public bool IsRead { get; set; }
    }

    public class ContentItemEntity
    {
        [BsonElement("ct")]
        public ContentType ContentType { get; set; }

        [BsonElement("n")]
        public string FileName { get; set; }

        [BsonElement("bts")]
        public string FileId { get; set; }

        [BsonIgnore]
        public byte[] FileBytes { get; set; } 
    }

    public enum ContentType
    {
        Image = 1
    }

    public enum MessageType
    {
        Base = 1,
        System = 2
    }
}
