using System;
using System.Collections.Generic;
using Funkmap.Messenger.Entities;

namespace Funkmap.Messenger.Contracts
{
    public class Message
    {
        public string Id { get; set; }
        public string DialogId { get; set; }
        public string Sender { get; set; }
        public string Text { get; set; }
        public DateTime DateTimeUtc { get; set; }

        public List<ContentItem> Content { get; set; }

        public bool IsNew { get; set; }

        public MessageType MessageType { get; set; }

        public List<string> ToParticipants { get; set; }
    }
}