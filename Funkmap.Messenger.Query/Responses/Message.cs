using System;
using Funkmap.Messenger.Entities;

namespace Funkmap.Messenger.Query.Responses
{
    public class Message
    {
        public string DialogId { get; set; }
        public string Sender { get; set; }
        public string Text { get; set; }
        public DateTime DateTimeUtc { get; set; }

        public ContentItem[] Content { get; set; }

        public bool IsNew { get; set; }

        public MessageType MessageType { get; set; }
    }
}
