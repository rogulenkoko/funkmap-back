using System;
using System.Collections.Generic;
using System.Linq;
using Funkmap.Common.Data.Mongo;
using Funkmap.Messenger.Data;
using Funkmap.Messenger.Data.Parameters;
using Funkmap.Messenger.Data.Repositories;
using Funkmap.Messenger.Data.Repositories.Abstract;
using Funkmap.Messenger.Entities;
using Funkmap.Messenger.Tests.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;

namespace Funkmap.Messenger.Tests
{
    [TestClass]
    public class DialogsRepositoryTest
    {
        private IDialogRepository _dialogRepository;

        private IMessageRepository _messageRepository;

        [TestInitialize]
        public void Initialize()
        {
            var db = MessengerDbProvider.DropAndCreateDatabase;

            var messagesCollection = db.GetCollection<MessageEntity>(MessengerCollectionNameProvider.MessagesCollectionName);

            _dialogRepository = new DialogRepository(db.GetCollection<DialogEntity>(MessengerCollectionNameProvider.DialogsCollectionName), messagesCollection);

            var fileStorage = new GridFsFileStorage(MessengerDbProvider.GetGridFsBucket(db));
            _messageRepository = new MessageRepository(messagesCollection, fileStorage);
        }

        [TestMethod]
        public void GetUserDialogs()
        {
            var login = "rogulenkoko";
            var dialogs = _dialogRepository.GetUserDialogsAsync(login).Result;
            Assert.AreEqual(dialogs.Count, 4);

            login = "test";
            dialogs = _dialogRepository.GetUserDialogsAsync(login).Result;
            Assert.AreEqual(dialogs.Count, 2);
        }

        [TestMethod]
        public void UpdateLastMessageDateTest()
        {
            var login = "rogulenkoko";
            var dialog = _dialogRepository.GetUserDialogsAsync(login).GetAwaiter().GetResult().Last();

            var parameter = new UpdateLastMessageDateParameter()
            {
                DialogId = dialog.Id.ToString(),
                Date = DateTime.Now
            };
            _dialogRepository.UpdateLastMessageDate(parameter).Wait();

            var updatedDialog = _dialogRepository.GetUserDialogsAsync(login).GetAwaiter().GetResult().First();

            Assert.AreEqual(updatedDialog.Id.ToString(), dialog.Id.ToString());
        }

        [TestMethod]
        public void CreateDialog()
        {
            var dialog = new DialogEntity()
            {
                LastMessageDate = DateTime.Now,
                Participants = new List<string>() { "qwert", "trewq"}
            };
            var id = _dialogRepository.CreateAndGetIdAsync(dialog).GetAwaiter().GetResult();
            Assert.AreNotEqual(id.ToString(), ObjectId.Empty.ToString());
        }
    }
}
