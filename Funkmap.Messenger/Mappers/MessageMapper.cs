using Funkmap.Messenger.Data.Entities;
using Funkmap.Messenger.Models;

namespace Funkmap.Messenger.Mappers
{
    public static class MessageMapper
    {
        public static Message ToModel(this MessageEntity source)
        {
            if (source == null) return null;
            return new Message()
            {
                Receiver = source.Receiver,
                Sender = source.Sender,
                DateTimeUtc = source.DateTimeUtc,
                Text = source.Text
            };
        }

        public static MessageEntity ToEntity(this Message source)
        {
            if (source == null) return null;
            return new MessageEntity()
            {
                Receiver = source.Receiver,
                Sender = source.Sender,
                Text = source.Text
            };
        }
    }
}
