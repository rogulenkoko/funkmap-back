using System.Collections.Generic;

namespace Funkmap.Messenger.Query.Responses
{
    public class UserDialogsResponse
    {
        public UserDialogsResponse(bool success, IReadOnlyCollection<DialogWithLastMessage> dialogs)
        {
            Success = success;
            Dialogs = dialogs;
        }

        public bool Success { get; }

        public IReadOnlyCollection<DialogWithLastMessage> Dialogs { get; }
    }
}
