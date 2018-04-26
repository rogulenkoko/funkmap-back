using System.Collections.Generic;

namespace Funkmap.Messenger.Query.Responses
{
    public class UserDialogsResponse
    {
        public UserDialogsResponse(bool success, IList<DialogWithLastMessage> dialogs)
        {
            Success = success;
            Dialogs = dialogs;
        }

        public bool Success { get; }

        public IList<DialogWithLastMessage> Dialogs { get; }
    }
}
