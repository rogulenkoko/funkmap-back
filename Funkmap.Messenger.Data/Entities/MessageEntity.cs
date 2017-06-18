using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Common.Data.Mongo.Abstract;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Funkmap.Messenger.Data.Entities
{
    public class MessageEntity : MongoEntity
    {
        public DateTime Date { get; set; }

        [BsonElement("sen")]
        public string Sender { get; set; }

        [BsonElement("cons")]
        public string Consumer { get; set; }

        [BsonElement("t")]
        public string Text { get; set; }

        [BsonElement("cont")]
        [BsonIgnoreIfDefault]
        public List<ContentItem> Content { get; set; }
    }

    public class ContentItem
    {
        [BsonElement("ct")]
        public ContentType ContentType { get; set; }

        [BsonElement("bts")]
        public BsonBinaryData Content { get; set; }
    }

    public enum ContentType
    {
        Image = 1
    }
}
