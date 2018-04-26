using System.Collections.Generic;
using Funkmap.Messenger.Contracts;

namespace Funkmap.Messenger.Query.Responses
{
    public class DialogMessagesResponse
    {
        public DialogMessagesResponse(bool success, ICollection<Message> messages)
        {
            Success = success;
            Messages = messages;
        }

        public bool Success { get; }

        public ICollection<Message> Messages { get; }
    }
}
