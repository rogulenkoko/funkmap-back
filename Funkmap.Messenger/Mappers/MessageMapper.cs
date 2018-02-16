using System.Linq;
using Funkmap.Messenger.Entities;
using Funkmap.Messenger.Models;
using Funkmap.Messenger.Query.Responses;

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

        public static MessageModel ToModel(this MessageEntity source)
        {
            if (source == null) return null;
            return new MessageModel()
            {
                Sender = source.Sender,
                DateTimeUtc = source.DateTimeUtc,
                Text = source.Text,
                IsNew = !source.IsRead,
                DialogId = source.DialogId.ToString(),
                MessageType = source.MessageType,
                Content = source.Content?.Select(x=>x.ToModel()).ToList()
            };
        }
    }
}
