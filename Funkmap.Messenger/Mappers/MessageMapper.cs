using System;
using System.Collections.Generic;
using System.Linq;
using Funkmap.Messenger.Data.Entities;
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
                DateTimeUtc = DateTime.UtcNow,
                ToParticipants = recievers
            };
        }
    }
}
