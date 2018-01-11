using Funkmap.Messenger.Entities;

namespace Funkmap.Messenger.Events.Dialogs
{
    public class DialogCreatedEvent
    {
        public DialogEntity Dialog { get; set; }
    }
}
