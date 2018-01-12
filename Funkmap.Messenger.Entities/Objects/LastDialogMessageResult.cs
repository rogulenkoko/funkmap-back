using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Funkmap.Messenger.Entities.Objects
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
