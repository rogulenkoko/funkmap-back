using System;
using System.Collections.Generic;
using Funkmap.Messenger.Entities;

namespace Funkmap.Messenger.Models
{
    public class MessageModel
    {
        public string DialogId { get; set; }
        public string Sender { get; set; }
        public string Text { get; set; }
        public DateTime DateTimeUtc { get; set; }

        public List<ContentItemModel> Content { get; set; }

        public bool IsNew { get; set; }

        public MessageType MessageType { get; set; }
    }
}
