using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Common.Data.Mongo.Abstract;
using MongoDB.Bson.Serialization.Attributes;

namespace Funkmap.Messenger.Data.Entities
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
