using System.Linq;
using Funkmap.Messenger;
using Funkmap.Messenger.Data.Entities;
using Funkmap.Messenger.Data.Parameters;
using Funkmap.Messenger.Data.Repositories;
using Funkmap.Messenger.Data.Repositories.Abstract;
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
    }
}
