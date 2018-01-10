using System.Collections.Generic;
using Funkmap.Messenger.Entities;
using Funkmap.Messenger.Models;
using MongoDB.Bson;

namespace Funkmap.Messenger.Mappers
{
    public static class MessageMapper
    {
        public static Message ToModel(this MessageEntity source)
        {
            if (source == null) return null;
            return new Message()
            {
                Sender = source.Sender,
                DateTimeUtc = source.DateTimeUtc,
                Text = source.Text,
                IsNew = !source.IsRead,
                DialogId = source.DialogId.ToString()
            };
        }

        public static MessageEntity ToEntity(this Message source, List<string> recievers)
        {
            if (source == null) return null;


            var content = new List<ContentItem>();

           // content.AddRange(source.Images.Select());

            return new MessageEntity()
            {
                Sender = source.Sender,
                Text = source.Text,
                DialogId = new ObjectId(source.DialogId),
                DateTimeUtc = source.DateTimeUtc,
                ToParticipants = recievers,
                IsRead = !source.IsNew
            };
        }
    }
}
