using System;
using System.Collections.Generic;
using Funkmap.Common.Data.Mongo.Abstract;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Funkmap.Messenger.Data.Entities
{
    public class MessageEntity : MongoEntity
    {
        public MessageEntity()
        {
            DateTimeUtc = DateTime.UtcNow;
            Content = new List<ContentItem>();
            ParticipantsWhoRead = new List<string>();
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
        public List<string> ParticipantsWhoRead { get; set; }

        [BsonElement("cont")]
        [BsonIgnoreIfDefault]
        public List<ContentItem> Content { get; set; }

        [BsonElement("ir")]
        public bool IsRead { get; set; }
    }

    public class ContentItem
    {
        [BsonElement("ct")]
        public ContentType ContentType { get; set; }

        [BsonElement("n")]
        public string FileName { get; set; }

        [BsonElement("bts")]
        public ObjectId FileId { get; set; }

        [BsonIgnore]
        public byte[] FileBytes { get; set; } 
    }

    public enum ContentType
    {
        Image = 1
    }
}
