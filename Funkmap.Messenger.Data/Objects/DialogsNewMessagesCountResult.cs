using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace Funkmap.Messenger.Data.Objects
{
    public class DialogsNewMessagesCountResult
    {
        public ObjectId DialogId { get; set; }
        public int NewMessagesCount { get; set; }
    }
}
