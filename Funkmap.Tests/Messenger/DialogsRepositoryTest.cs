using System;
using System.Collections.Generic;
using System.Linq;
using Funkmap.Messenger;
using Funkmap.Messenger.Data.Entities;
using Funkmap.Messenger.Data.Parameters;
using Funkmap.Messenger.Data.Repositories;
using Funkmap.Messenger.Data.Repositories.Abstract;
using Funkmap.Tests.Images;
using Funkmap.Tests.Messenger.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver.GridFS;

namespace Funkmap.Tests.Messenger
{
    [TestClass]
    public class DialogsRepositoryTest
    {
        private IDialogRepository _dialogRepository;

        [TestInitialize]
        public void Initialize()
        {
            var db = MessengerDbProvider.DropAndCreateDatabase;
            _dialogRepository = new DialogRepository(db.GetCollection<DialogEntity>(MessengerCollectionNameProvider.DialogsCollectionName), new GridFSBucket(db));
        }

        [TestMethod]
        public void GetUserDialogs()
        {
            var parameter = new UserDialogsParameter()
            {
                Login = "rogulenkoko",
                Skip = 0,
                Take = 100
            };
            var dialogs = _dialogRepository.GetUserDialogsAsync(parameter).Result;
            Assert.AreEqual(dialogs.Count, 4);

            parameter.Login = "test";
            dialogs = _dialogRepository.GetUserDialogsAsync(parameter).Result;
            Assert.AreEqual(dialogs.Count, 2);
        }

        [TestMethod]
        public void GetDialogMessages()
        {
            var dialogsParameter = new UserDialogsParameter()
            {
                Login = "rogulenkoko",
                Skip = 0,
                Take = 100
            };

            var dialog = _dialogRepository.GetUserDialogsAsync(dialogsParameter).Result.First();

            var parameter = new DialogMessagesParameter()
            {
                DialogId = dialog.Id.ToString(),
                Take = 4,
                Skip = 0
            };

            var messages = _dialogRepository.GetDialogMessagesAsync(parameter).Result;
            Assert.AreEqual(messages.Count, 4);

            parameter.Skip = 4;
            messages = _dialogRepository.GetDialogMessagesAsync(parameter).Result;
            Assert.AreEqual(messages.Count, 2);

            parameter = new DialogMessagesParameter()
            {
                DialogId = dialog.Id.ToString(),
                Take = 5,
                Skip = 0
            };

            messages = _dialogRepository.GetDialogMessagesAsync(parameter).Result;
            Assert.AreEqual(messages.Count, 5);

            parameter.Skip = 5;
            messages = _dialogRepository.GetDialogMessagesAsync(parameter).Result;
            Assert.AreEqual(messages.Count, 1);
        }

        [TestMethod]
        public void CreateMessageTest()
        {
            var dialogsParameter = new UserDialogsParameter()
            {
                Login = "rogulenkoko",
                Skip = 0,
                Take = 100
            };
            var dialogs = _dialogRepository.GetUserDialogsAsync(dialogsParameter).Result;
            var dialog = dialogs.First();

            var dialogMessagesParameter = new DialogMessagesParameter()
            {
                DialogId = dialog.Id.ToString(),
                Skip = 0,
                Take = 100
            };
            var dialogMessages = _dialogRepository.GetDialogMessagesAsync(dialogMessagesParameter).Result;

            var message = new MessageEntity()
            {
                Text = "aaaaa",
                Sender = "rogulenkoko"
            };

            _dialogRepository.AddMessage(dialog.Id.ToString(), message).Wait();

            var newDialogMessages = _dialogRepository.GetDialogMessagesAsync(dialogMessagesParameter).Result;

            Assert.AreNotEqual(dialogMessages.Count, newDialogMessages.Count);
            Assert.AreEqual(dialogMessages.Count + 1, newDialogMessages.Count);
        }

        [TestMethod]
        public void MessageContentTest()
        {
            var dialogsParameter = new UserDialogsParameter()
            {
                Login = "rogulenkoko",
                Skip = 0,
                Take = 100
            };
            var dialogs = _dialogRepository.GetUserDialogsAsync(dialogsParameter).Result;
            var dialog = dialogs.First();

            var filename = "beatles-avatar.jpg";
            var image = ImageProvider.GetAvatar(filename);

            var message = new MessageEntity()
            {
                Text = "aaaaa",
                Sender = "rogulenkoko",
                Content = new List<ContentItem>()
                {
                    new ContentItem()
                    {
                        FileBytes = image,
                        FileName = filename,
                        ContentType = ContentType.Image
                    }
                }
            };

            _dialogRepository.AddMessage(dialog.Id.ToString(), message).Wait();

            var dialogMessagesParameter = new DialogMessagesParameter()
            {
                DialogId = dialog.Id.ToString(),
                Skip = 0,
                Take = 100
            };
            var dialogMessages = _dialogRepository.GetDialogMessagesAsync(dialogMessagesParameter).Result;


            var files = _dialogRepository.GetMessagesContent(dialogMessages
                .SelectMany(x => x.Content.Select(y => y.FileId.ToString()))
                .ToArray());

            Assert.AreEqual(files.Count, 1);

            var file = files.First();
            Assert.AreEqual(file.FileBytes.Length, image.Length);
        }

        [TestMethod]
        public void GetDialogsWithNewMEssages()
        {
            var parameter = new DialogsWithNewMessagesParameter()
            {
                Login = "rogulenkoko",
                LastVisitDate = DateTime.Now.AddMinutes(-30)
            };

            var result = _dialogRepository.GetDialogsWithNewMessages(parameter).GetAwaiter().GetResult();

            Assert.AreEqual(result.Count, 1);
        }
    }


    
}
