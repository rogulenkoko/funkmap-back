using System;
using System.Collections.Generic;
using Funkmap.Messenger;
using Funkmap.Messenger.Data.Entities;
using Funkmap.Messenger.Data.Repositories;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

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
            var dialogsRepository = new DialogRepository(_database.GetCollection<DialogEntity>(MessengerCollectionNameProvider.DialogsCollectionName), new GridFSBucket(_database));

            var messages = new List<MessageEntity>()
            {
                new MessageEntity() { Sender = "rogulenkoko", Text = "первое сообщение",DateTimeUtc = DateTime.Now.AddHours(-2) },
                new MessageEntity() { Sender = "test", Text = "привет",DateTimeUtc = DateTime.Now.AddHours(-1) },
                new MessageEntity() { Sender = "rogulenkoko", Text = "привет",DateTimeUtc = DateTime.Now.AddMinutes(7) },
                new MessageEntity() { Sender = "rogulenkoko", Text = "привет",DateTimeUtc = DateTime.Now.AddMinutes(10) },
                new MessageEntity() { Sender = "rogulenkoko", Text = "привет",DateTimeUtc= DateTime.Now.AddMinutes(12) },
                new MessageEntity() { Sender = "rogulenkoko", Text = "последнее сообщение",DateTimeUtc= DateTime.Now.AddMinutes(20) }
            };

            var dialogs = new List<DialogEntity>()
            {
                new DialogEntity() {Participants = new List<string>() {"rogulenkoko", "test"}, Messages = messages, MessagesCount = messages.Count},
                new DialogEntity() {Participants = new List<string>() {"rogulenkoko", "qwe"}},
                new DialogEntity() {Participants = new List<string>() {"qwe", "test"}},
                new DialogEntity() {Participants = new List<string>() {"asd", "zxc"}},
                new DialogEntity() {Participants = new List<string>() {"rogulenkoko", "zxc"}},
                new DialogEntity() {Participants = new List<string>() { "zzzzzz", "rogulenkoko"}},
                new DialogEntity() {Participants = new List<string>() {"zzzzzz", "zxc"}},
            };
            
            foreach (var dialog in dialogs)
            {
                dialogsRepository.CreateAsync(dialog).Wait();
            }
        }
    }
}
