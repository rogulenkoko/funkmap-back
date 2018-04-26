using System.Collections.Generic;

namespace Funkmap.Messenger.Contracts.Events.Messages
{
    public class MessageSavedCompleteEvent
    {
        public bool Success { get; set; }
        public Message Message { get; set; }
        public List<string> DialogParticipants { get; set; }
    }
}
