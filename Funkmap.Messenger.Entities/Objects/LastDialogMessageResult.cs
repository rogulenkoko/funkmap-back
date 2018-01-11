using System.Collections.Generic;
using Funkmap.Messenger.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Funkmap.Messenger.Data.Objects
{
    public class LastDialogMessageResult
    {
        public ObjectId DialogId { get; set; }
        public MessageEntity LastMessage { get; set; }
    }

    public class DialogLookup : DialogEntity
    {
        [BsonElement("mes")]
        public List<MessageEntity> LastMessages { get; set; }
    }
}
