
using System;
using Funkmap.Messenger.Data.Entities;

namespace Funkmap.Messenger.Models
{
    public class Message
    {
        public string DialogId { get; set; }
        public string Sender { get; set; }
        public string Text { get; set; }
        public DateTime DateTimeUtc { get; set; }

        public ContentItem[] Images { get; set; }
    }
}
