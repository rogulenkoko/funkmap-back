
using Funkmap.Messenger.Entities;

namespace Funkmap.Messenger.Events.Dialogs
{
    public class DialogUpdatedEvent
    {
        public DialogUpdatedEvent(DialogEntity dialog)
        {
            Dialog = dialog;
        }
        public DialogEntity Dialog { get; }
    }
}

