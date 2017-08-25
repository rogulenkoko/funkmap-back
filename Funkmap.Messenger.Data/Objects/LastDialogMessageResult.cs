using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Messenger.Data.Entities;
using MongoDB.Bson;

namespace Funkmap.Messenger.Data.Objects
{
    public class LastDialogMessageResult
    {
        public ObjectId DialogId { get; set; }
        public BsonDocument LastMessage { get; set; }
    }
}
