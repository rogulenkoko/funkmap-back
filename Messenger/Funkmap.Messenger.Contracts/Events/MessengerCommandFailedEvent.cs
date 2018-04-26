using Funkmap.Common.Cqrs;

namespace Funkmap.Messenger.Contracts.Events
{
    public class MessengerCommandFailedEvent : CommandFailedEvent
    {
        public string Sender { get; set; }
    }
}
