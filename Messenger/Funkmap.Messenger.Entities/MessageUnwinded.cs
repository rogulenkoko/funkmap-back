using System.Collections.Generic;
using Funkmap.Common.Data.Mongo.Abstract;
using MongoDB.Bson.Serialization.Attributes;

namespace Funkmap.Messenger.Entities
{
    public class MessageUnwinded : MongoEntity
    {

        public MessageUnwinded()
        {
            Participants = new List<string>();
        }

        [BsonElement("mc")]
        public int MessageCount { get; set; }

        [BsonElement("m")]
        public MessageEntity Message { get; set; }

        [BsonElement("prtcpnts")]
        public List<string> Participants { get; set; }
    }
}
