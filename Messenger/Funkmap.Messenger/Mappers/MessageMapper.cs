using System.Linq;
using Funkmap.Messenger.Contracts;
using Funkmap.Messenger.Models;

namespace Funkmap.Messenger.Mappers
{
    public static class MessageMapper
    {
        public static MessageModel ToModel(this Message source)
        {
            if (source == null) return null;
            return new MessageModel()
            {
                Sender = source.Sender,
                DateTimeUtc = source.DateTimeUtc,
                Text = source.Text,
                IsNew = source.IsNew,
                DialogId = source.DialogId,
                MessageType = source.MessageType,
                Content = source.Content?.Select(x=>x.ToModel()).ToList()
            };
        }
    }
}
