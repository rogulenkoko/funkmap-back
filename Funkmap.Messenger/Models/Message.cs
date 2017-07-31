
using System;

namespace Funkmap.Messenger.Models
{
    public class Message
    {
        public string Sender { get; set; }
        public string Receiver { get; set; }
        public string Text { get; set; }
        public DateTime DateTimeUtc { get; set; }
    }
}
