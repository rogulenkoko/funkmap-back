using Funkmap.Common.Cqrs;

namespace Funkmap.Messenger.Events
{
    public class MessengerCommandFailedEvent : CommandFailedEvent
    {
        public string Sender { get; set; }
    }
}
