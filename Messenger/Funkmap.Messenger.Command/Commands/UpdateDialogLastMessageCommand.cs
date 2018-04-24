using System;
using Funkmap.Messenger.Entities;

namespace Funkmap.Messenger.Command.Commands
{
    public class UpdateDialogLastMessageCommand
    {
        public UpdateDialogLastMessageCommand(string dialogId, DateTime lastMesssageDateTime)
        {
            DialogId = dialogId;
            LastMesssageDateTime = lastMesssageDateTime;
        }

        public string DialogId { get; }
        public DateTime LastMesssageDateTime{ get; }

        public MessageEntity Message { get; set; }
    }
}
