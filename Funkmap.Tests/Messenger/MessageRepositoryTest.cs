using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Messenger;
using Funkmap.Messenger.Data.Entities;
using Funkmap.Messenger.Data.Objects;
using Funkmap.Messenger.Data.Parameters;
using Funkmap.Messenger.Data.Repositories;
using Funkmap.Messenger.Data.Repositories.Abstract;
using Funkmap.Tests.Images;
using Funkmap.Tests.Messenger.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Funkmap.Tests.Messenger
{
    [TestClass]
    public class MessageRepositoryTest
    {
        private IMessageRepository _messageRepository;

        private IDialogRepository _dialogRepository;

        [TestInitialize]
        public void Initialize()
        {
            var db = MessengerDbProvider.DropAndCreateDatabase;
            _messageRepository = new MessageRepository(db.GetCollection<MessageEntity>(MessengerCollectionNameProvider.MessagesCollectionName),
                MessengerDbProvider.GetGridFsBucket(db));
            _dialogRepository = new DialogRepository(db.GetCollection<DialogEntity>(MessengerCollectionNameProvider.DialogsCollectionName));
        }

        [TestMethod]
        public void GetNewMessagesTest()
        {
            var dialogsParameter = new UserDialogsParameter()
            {
                Login = "rogulenkoko",
                Skip = 0,
                Take = 100
            };

            var dialogs = _dialogRepository.GetUserDialogsAsync(dialogsParameter).GetAwaiter().GetResult();
            var param = new DialogsNewMessagesParameter()
            {
                Login = dialogsParameter.Login,
                DialogIds = dialogs.Select(x=>x.Id.ToString()).ToList()
            };
            var count = _messageRepository.GetDialogsWithNewMessagesCountAsync(param).GetAwaiter().GetResult();
            Assert.AreEqual(count, 1);
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
                Skip = 0,
                UserLogin = "rogulenkoko"
            };

            var messages = _messageRepository.GetDialogMessagesAsync(parameter).Result; 
            Assert.AreEqual(messages.Count, 4);


            var newMessagesParameter = new DialogsNewMessagesParameter()
            {
                Login = parameter.UserLogin,
                DialogIds = new List<string>() { parameter.DialogId } 
            };
            ICollection<DialogsNewMessagesCountResult> newMessagesCount = _messageRepository.GetDialogNewMessagesCount(newMessagesParameter)
                .GetAwaiter().GetResult();
            var myDialogResult = newMessagesCount.Single(x => x.DialogId.ToString() == parameter.DialogId);

            Assert.AreEqual(myDialogResult.NewMessagesCount, 1);

            parameter.Skip = 4;
            messages = _messageRepository.GetDialogMessagesAsync(parameter).Result;
            Assert.AreEqual(messages.Count, 2);

            parameter = new DialogMessagesParameter()
            {
                DialogId = dialog.Id.ToString(),
                Take = 5,
                Skip = 0,
                UserLogin = "rogulenkoko"
            };

            messages = _messageRepository.GetDialogMessagesAsync(parameter).Result;
            Assert.AreEqual(messages.Count, 5);

            parameter.Skip = 5;
            messages = _messageRepository.GetDialogMessagesAsync(parameter).Result;
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
                Take = 100,
                UserLogin = dialogsParameter.Login
            };
            var dialogMessages = _messageRepository.GetDialogMessagesAsync(dialogMessagesParameter).Result;

            var message = new MessageEntity()
            {
                Text = "aaaaa",
                Sender = "rogulenkoko",
                DialogId = dialog.Id
            };

            _messageRepository.AddMessage(message).Wait();

            var newDialogMessages = _messageRepository.GetDialogMessagesAsync(dialogMessagesParameter).Result;

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
                DialogId = dialog.Id,
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

            _messageRepository.AddMessage(message).Wait();

            var dialogMessagesParameter = new DialogMessagesParameter()
            {
                DialogId = dialog.Id.ToString(),
                Skip = 0,
                Take = 100,
                UserLogin = "rogulenkoko"
            };
            var dialogMessages = _messageRepository.GetDialogMessagesAsync(dialogMessagesParameter).Result;


            var files = _messageRepository.GetMessagesContent(dialogMessages
                .SelectMany(x => x.Content.Select(y => y.FileId.ToString()))
                .ToArray());

            Assert.AreEqual(files.Count, 1);

            var file = files.First();
            Assert.AreEqual(file.FileBytes.Length, image.Length);
        }

        [TestMethod]
        public void GetLastDialogMessage()
        {
            var dialogsParameter = new UserDialogsParameter()
            {
                Login = "rogulenkoko",
                Skip = 0,
                Take = 100
            };
            var dialogs = _dialogRepository.GetUserDialogsAsync(dialogsParameter).Result;
            var dialog = dialogs.First();

            var parameter = new DialogMessagesParameter()
            {
                DialogId = dialog.Id.ToString(),
                Take = Int32.MaxValue,
                Skip = 0,
                UserLogin = "rogulenkoko"
            };

            var allMessages = _messageRepository.GetDialogMessagesAsync(parameter).GetAwaiter().GetResult();
            var trueLastMessage = allMessages.Last();

            var lastMessage = _messageRepository.GetLastDialogMessage(dialog.Id.ToString()).GetAwaiter().GetResult();

            Assert.AreEqual(lastMessage.Id.ToString(), trueLastMessage.Id.ToString());
        }

    }
}
