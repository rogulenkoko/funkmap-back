using System;
using System.Collections.Generic;
using System.Linq;
using Funkmap.Messenger.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using Funkmap.Messenger.Command;

namespace Funkmap.Messenger.Tests.Data
{
    public class MessengerDataSeeder
    {
        private readonly IMongoDatabase _database;

        public MessengerDataSeeder(IMongoDatabase database)
        {
            _database = database;
        }

        public void SeedData()
        {
            var messagesCollection = _database.GetCollection<MessageEntity>(MessengerCollectionNameProvider.MessagesCollectionName);
            var dialogsCollection = _database.GetCollection<DialogEntity>(MessengerCollectionNameProvider.DialogsCollectionName);

            var dialogs = new List<DialogEntity>()
            {
                new DialogEntity() {Participants = new List<string>() {"rogulenkoko", "test"}},
                new DialogEntity() {Participants = new List<string>() {"rogulenkoko", "bandmap"}}
            };

            dialogsCollection.InsertManyAsync(dialogs).GetAwaiter().GetResult();

            var random = new Random();
            var now = DateTime.Now;
            var messages = Enumerable.Range(0, 200).Select(x=>
            {
                var randomDialog = dialogs.ElementAt(random.Next(0, dialogs.Count));

                var sender = randomDialog.Participants.ElementAt(random.Next(0, randomDialog.Participants.Count));

                return new MessageEntity()
                {
                    DialogId = randomDialog.Id,
                    Sender = sender,
                    ToParticipants = randomDialog.Participants.Where(y=> y != sender).ToList(),
                    MessageType = MessageType.Base,
                    Text = Guid.NewGuid().ToString(),
                    DateTimeUtc = now.AddHours(-x)
                };
            });

            messagesCollection.InsertManyAsync(messages).GetAwaiter().GetResult();

            foreach (var dialog in dialogs)
            {
                var lastMessage = messages.First(x => x.DialogId == dialog.Id);
                dialog.LastMessage = lastMessage;
                dialog.LastMessageDate = lastMessage.DateTimeUtc;
                dialogsCollection.ReplaceOneAsync(x => x.Id == dialog.Id, dialog).GetAwaiter().GetResult();
            }

        }
    }
}
