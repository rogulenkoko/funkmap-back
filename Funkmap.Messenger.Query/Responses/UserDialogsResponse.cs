using System.Collections.Generic;
using Funkmap.Messenger.Entities;

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

    public class DialogWithLastMessage
    {
        public string DialogId { get; set; }
        public string Name { get; set; }
        public List<string> Participants { get; set; }

        public Message LastMessage { get; set; }

        public string CreatorLogin { get; set; }

        public int NewMessagesCount { get; set; }

        public DialogType DialogType { get; set; }
    }
}
