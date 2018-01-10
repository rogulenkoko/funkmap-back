using Funkmap.Messenger.Entities;

namespace Funkmap.Messenger.Command.Commands
{
    public class UpdateDialogLastMessageCommand
    {
        public UpdateDialogLastMessageCommand(string dialogId, MessageEntity message)
        {
            DialogId = dialogId;
            Message = message;
        }

        public string DialogId { get; }
        public MessageEntity Message{ get; }
    }
}
