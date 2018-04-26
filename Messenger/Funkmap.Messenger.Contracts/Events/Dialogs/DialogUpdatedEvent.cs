
using Funkmap.Messenger.Entities;

namespace Funkmap.Messenger.Contracts.Events.Dialogs
{
    public class DialogUpdatedEvent
    {
        public DialogUpdatedEvent(Dialog dialog)
        {
            Dialog = dialog;
        }
        public Dialog Dialog { get; }
    }
}

