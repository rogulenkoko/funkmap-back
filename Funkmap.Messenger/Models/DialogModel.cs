using System.Collections.Generic;

namespace Funkmap.Messenger.Models
{
    public class DialogModel
    {
        public string DialogId { get; set; }
        public string Name { get; set; }
        public byte[] Avatar { get; set; }
        public List<string> Participants { get; set; }

        public MessageModel LastMessage { get; set; }

        public string CreatorLogin { get; set; }

    }
}
