using MongoDB.Bson;

namespace Funkmap.Messenger.Query.Responses
{
    public class DialogsNewMessagesCountResult
    {
        public string DialogId { get; set; }
        public int NewMessagesCount { get; set; }
    }

    public class DialogsNewMessagesCountResultEntity
    {
        public ObjectId DialogId { get; set; }
        public int NewMessagesCount { get; set; }
    }
}
