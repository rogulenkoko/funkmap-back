using System.Collections.Generic;
using Funkmap.Messenger.Entities;

namespace Funkmap.Messenger.Events.Messages
{
    public class MessageSavedCompleteEvent
    {
        public bool Success { get; set; }
        public MessageEntity Message { get; set; }

        public List<string> DialogParticipants { get; set; }
    }
}
