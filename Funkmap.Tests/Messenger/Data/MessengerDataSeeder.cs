using System;
using System.Collections.Generic;
using Funkmap.Common.Data.Mongo;
using Funkmap.Messenger;
using Funkmap.Messenger.Data;
using Funkmap.Messenger.Data.Entities;
using Funkmap.Messenger.Data.Repositories;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Funkmap.Tests.Messenger.Data
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
            SeedDialogs();
        }

        private void SeedDialogs()
        {
            var messagesCollection = _database.GetCollection<MessageEntity>(MessengerCollectionNameProvider.MessagesCollectionName);
            var dialogsRepository = new DialogRepository(_database.GetCollection<DialogEntity>(MessengerCollectionNameProvider.DialogsCollectionName), messagesCollection);

            var fileStorage = new GridFsFileStorage(MessengerDbProvider.GetGridFsBucket(_database));
            var messagesRepository = new MessageRepository(messagesCollection, fileStorage);

            var dialogId = ObjectId.GenerateNewId();
            var participants = new List<string>() {"test", "rogulenkoko"};

            var messages = new List<MessageEntity>()
            {
                new MessageEntity() { Sender = "rogulenkoko", Text = "первое сообщение",DateTimeUtc = DateTime.Now.AddHours(-2), DialogId = dialogId, ToParticipants = new List<string>() {"test"}},
                new MessageEntity() { Sender = "test", Text = "привет",DateTimeUtc = DateTime.Now.AddHours(-1), DialogId = dialogId, ToParticipants = new List<string>() {"rogulenkoko"}},
                new MessageEntity() { Sender = "rogulenkoko", Text = "привет",DateTimeUtc = DateTime.Now.AddMinutes(7), DialogId = dialogId, ToParticipants = new List<string>() {"test"}},
                new MessageEntity() { Sender = "rogulenkoko", Text = "привет",DateTimeUtc = DateTime.Now.AddMinutes(10), DialogId = dialogId, ToParticipants = new List<string>() {"test"}},
                new MessageEntity() { Sender = "rogulenkoko", Text = "привет",DateTimeUtc= DateTime.Now.AddMinutes(12), DialogId = dialogId, ToParticipants = new List<string>() {"test"}},
                new MessageEntity() { Sender = "rogulenkoko", Text = "последнее сообщение",DateTimeUtc= DateTime.Now.AddMinutes(20), DialogId = dialogId, ToParticipants = new List<string>() {"test"}}
            };

            foreach (var message in messages)
            {
                messagesRepository.AddMessage(message).Wait();
            }
            

            var dialogs = new List<DialogEntity>()
            {
                new DialogEntity() {Participants = participants, Id = dialogId},
                new DialogEntity() {Participants = new List<string>() {"rogulenkoko", "qwe"}},
                new DialogEntity() {Participants = new List<string>() {"qwe", "test"}},
                new DialogEntity() {Participants = new List<string>() {"asd", "zxc"}},
                new DialogEntity() {Participants = new List<string>() {"rogulenkoko", "zxc"}},
                new DialogEntity() {Participants = new List<string>() { "zzzzzz", "rogulenkoko"}},
                new DialogEntity() {Participants = new List<string>() {"zzzzzz", "zxc"}},
            };
            
            foreach (var dialog in dialogs)
            {
                dialogsRepository.CreateAndGetIdAsync(dialog).Wait();
            }
        }
    }
}
