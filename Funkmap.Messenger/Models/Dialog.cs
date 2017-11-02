using System.Collections.Generic;

namespace Funkmap.Messenger.Models
{
    public class Dialog
    {
        public string DialogId { get; set; }
        public string Name { get; set; }
        public byte[] Avatar { get; set; }
        public List<string> Participants { get; set; }

        public Message LastMessage { get; set; }

        public string CreatorLogin { get; set; }

    }
}
