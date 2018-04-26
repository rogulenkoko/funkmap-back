
using Funkmap.Messenger.Entities;

namespace Funkmap.Messenger.Contracts.Events.Messages
{
    public class ContentUploadedEvent
    {
        public ContentUploadedEvent(ContentType type, string name, string dataUrl, string sender)
        {
            Name = name;
            DataUrl = dataUrl;
            Sender = sender;
            ContentType = type;
        }

        public string Name { get; }
        public string DataUrl { get; }
        public string Sender { get; }
        public ContentType ContentType { get; }
    }
}
