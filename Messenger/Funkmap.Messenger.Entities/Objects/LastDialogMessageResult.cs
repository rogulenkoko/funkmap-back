using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace Funkmap.Messenger.Entities.Objects
{
    public class DialogLookup : DialogEntity
    {
        [BsonElement("mes")]
        public List<MessageEntity> LastMessages { get; set; }
    }
}
