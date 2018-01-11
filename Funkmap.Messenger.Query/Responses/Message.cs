using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funkmap.Messenger.Query.Responses
{
    public class Message
    {
        public string DialogId { get; set; }
        public string Sender { get; set; }
        public string Text { get; set; }
        public DateTime DateTimeUtc { get; set; }

        public ContentItem[] Images { get; set; }

        public bool IsNew { get; set; }
    }
}
