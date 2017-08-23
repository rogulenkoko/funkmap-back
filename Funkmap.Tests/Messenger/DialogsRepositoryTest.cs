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
using MongoDB.Bson;
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
            _dialogRepository = new DialogRepository(db.GetCollection<DialogEntity>(MessengerCollectionNameProvider.DialogsCollectionName));
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
        public void UpdateLastMessageDateTest()
        {
            var dialogsParameter = new UserDialogsParameter()
            {
                Login = "rogulenkoko",
                Skip = 0,
                Take = 100
            };
            var dialog = _dialogRepository.GetUserDialogsAsync(dialogsParameter).GetAwaiter().GetResult().Last();

            var parameter = new UpdateLastMessageDateParameter()
            {
                DialogId = dialog.Id.ToString(),
                Date = DateTime.Now
            };
            _dialogRepository.UpdateLastMessageDate(parameter).Wait();

            var updatedDialog = _dialogRepository.GetUserDialogsAsync(dialogsParameter).GetAwaiter().GetResult().First();

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
            var id = _dialogRepository.CreateAsync(dialog).GetAwaiter().GetResult();
            Assert.AreNotEqual(id.ToString(), ObjectId.Empty.ToString());
        }
    }
}
