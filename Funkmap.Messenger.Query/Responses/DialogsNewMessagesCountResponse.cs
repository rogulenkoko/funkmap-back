using System.Collections.Generic;
using Funkmap.Messenger.Entities.Objects;

namespace Funkmap.Messenger.Query.Responses
{
    public class DialogsNewMessagesCountResponse
    {
        public DialogsNewMessagesCountResponse(bool success, ICollection<DialogsNewMessagesCountResult> countResults)
        {
            Success = success;
            CountResults = countResults;
        }
        public bool Success { get; }
        public ICollection<DialogsNewMessagesCountResult> CountResults { get; }
    }
}
