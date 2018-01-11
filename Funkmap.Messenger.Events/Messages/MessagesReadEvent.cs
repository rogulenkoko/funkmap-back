using System;
using System.Collections.Generic;

namespace Funkmap.Messenger.Events.Messages
{
    public class MessagesReadEvent
    {
        public DateTime ReadTime { get; set; }
        public string DialogId { get; set; }
        public ICollection<string> DialogMembers { get; set; }

        public string UserLogin { get; set; }
    }
}
