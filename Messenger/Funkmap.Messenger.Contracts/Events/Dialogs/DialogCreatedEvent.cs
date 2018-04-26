namespace Funkmap.Messenger.Contracts.Events.Dialogs
{
    public class DialogCreatedEvent
    {
        public Dialog Dialog { get; set; }

        public string Sender { get; set; }
    }
}
