using System.Linq;
using Funkmap.Messenger.Contracts;
using MongoDB.Bson;

namespace Funkmap.Messenger.Entities.Mappers
{
    public static class MessageMapper
    {
        public static Message ToModel(this MessageEntity source)
        {
            if (source == null) return null;
            return new Message
            {
                Sender = source.Sender,
                DateTimeUtc = source.DateTimeUtc,
                Text = source.Text,
                IsNew = !source.IsRead,
                DialogId = source.DialogId.ToString(),
                MessageType = source.MessageType,
                Content = source.Content?.Select(x => x.ToModel()).ToList(),
                Id = source.Id.ToString(),
                ToParticipants = source.ToParticipants
            };
        }

        public static MessageEntity ToEntity(this Message source)
        {
            if (source == null) return null;
            return new MessageEntity()
            {

                DialogId = new ObjectId(source.DialogId),
                Id = new ObjectId(source.Id),
                Sender = source.Sender,
                Content = source.Content?.Select(x=>x.ToEntity()).ToList(),
                Text = source.Text,
                MessageType = source.MessageType,
                DateTimeUtc = source.DateTimeUtc,
                IsRead = !source.IsNew,
                ToParticipants = source.ToParticipants
            };
        }
    }
}
