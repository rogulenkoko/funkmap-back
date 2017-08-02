using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Messenger;
using Funkmap.Messenger.Data.Entities;
using Funkmap.Messenger.Data.Parameters;
using Funkmap.Messenger.Data.Repositories;
using Funkmap.Messenger.Data.Repositories.Abstract;
using Funkmap.Tests.Funkmap.Data;
using Funkmap.Tests.Messenger.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Funkmap.Tests.Messenger
{
    [TestClass]
    public class DialogsRepositoryTest
    {
        private IDialogRepository _dialogRepository;

        [TestInitialize]
        public void Initialize()
        {
            _dialogRepository = new DialogRepository(MessengerDbProvider.DropAndCreateDatabase.GetCollection<DialogEntity>(MessengerCollectionNameProvider.DialogsCollectionName));
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
            var parameter = new DialogMessagesParameter()
            {
                Members = new []{"rogulenkoko", "test"},
                Take = 4,
                Skip = 0
            };

            var messages = _dialogRepository.GetDialogMessagesAsync(parameter).Result;
            Assert.AreEqual(messages.Count, 4);

            parameter.Skip = 4;
            messages = _dialogRepository.GetDialogMessagesAsync(parameter).Result;
            Assert.AreEqual(messages.Count, 2);
        }
    }
}
