using System;
using System.Collections.Generic;
using Funkmap.Messenger;
using Funkmap.Messenger.Data.Entities;
using Funkmap.Messenger.Data.Repositories;
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
            SeedMessages();
            SeedDialogs();
        }

        private void SeedMessages()
        {
            var messageRepository = new MessageRepository(_database.GetCollection<MessageEntity>(MessengerCollectionNameProvider.MessegesCollectionName));
            var messages = new List<MessageEntity>()
            {
                new MessageEntity() {Receiver = "test", Sender = "rogulenkoko", Text = "привет",DateTimeUtc = DateTime.Now.AddMinutes(2) },
                new MessageEntity() {Receiver = "rogulenkoko", Sender = "test", Text = "привет",DateTimeUtc = DateTime.Now.AddMinutes(5) },
                new MessageEntity() {Receiver = "test", Sender = "rogulenkoko", Text = "привет",DateTimeUtc = DateTime.Now.AddMinutes(1) },
                new MessageEntity() {Receiver = "test", Sender = "rogulenkoko", Text = "привет",DateTimeUtc = DateTime.Now.AddMinutes(7) },
                new MessageEntity() {Receiver = "asd", Sender = "rogulenkoko", Text = "привет",DateTimeUtc= DateTime.Now.AddMinutes(2) },
                new MessageEntity() {Receiver = "test", Sender = "asd", Text = "привет",DateTimeUtc= DateTime.Now.AddMinutes(2) },
                new MessageEntity() {Receiver = "test", Sender = "rogulenkoko", Text = "привет",DateTimeUtc= DateTime.Now },
                new MessageEntity() {Receiver = "rogulenkoko", Sender = "rogulenkoko", Text = "с самим собой",DateTimeUtc= DateTime.Now }
            };
            foreach (var message in messages)
            {
                messageRepository.CreateAsync(message).Wait();
            }
        }

        private void SeedDialogs()
        {
            var dialogsRepository = new DialogRepository(_database.GetCollection<DialogEntity>(MessengerCollectionNameProvider.DialogsCollectionName));

            var dialogs = new List<DialogEntity>()
            {
                new DialogEntity() {Participants = new List<string>() {"rogulenkoko", "test"}},
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
