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
            var messagesCollection = db.GetCollection<MessageEntity>(MessengerCollectionNameProvider.MessagesCollectionName);

            _dialogRepository = new DialogRepository(db.GetCollection<DialogEntity>(MessengerCollectionNameProvider.DialogsCollectionName), messagesCollection);

            _messageRepository = new MessageRepository(messagesCollection, MessengerDbProvider.GetGridFsBucket(db));
        }

        [TestMethod]
        public void GetNewMessagesTest()
        {
            var login = "rogulenkoko";

            var dialogs = _dialogRepository.GetUserDialogsAsync(login).GetAwaiter().GetResult();
            var param = new DialogsNewMessagesParameter()
            {
                Login = login,
                DialogIds = dialogs.Select(x=>x.Id.ToString()).ToList()
            };
            var count = _messageRepository.GetDialogsWithNewMessagesAsync(param).GetAwaiter().GetResult();
            Assert.AreEqual(count.Count, 1);
        }

        [TestMethod]
        public void GetDialogMessages()
        {
            var login = "rogulenkoko";
            var dialog = _dialogRepository.GetUserDialogsAsync(login).Result.First();

            var parameter = new DialogMessagesParameter()
            {
                DialogId = dialog.Id.ToString(),
                Take = 4,
                Skip = 0,
                UserLogin = "rogulenkoko"
            };

            var messages = _messageRepository.GetDialogMessagesAsync(parameter).Result; 
            Assert.AreEqual(messages.Count, 4);

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
            var login = "rogulenkoko";
            var dialogs = _dialogRepository.GetUserDialogsAsync(login).Result;
            var dialog = dialogs.First();

            var dialogMessagesParameter = new DialogMessagesParameter()
            {
                DialogId = dialog.Id.ToString(),
                Skip = 0,
                Take = 100,
                UserLogin = login
            };
            var dialogMessages = _messageRepository.GetDialogMessagesAsync(dialogMessagesParameter).Result;

            var message = new MessageEntity()
            {
                Text = "aaaaa",
                Sender = login,
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
            var login = "rogulenkoko";
            var dialogs = _dialogRepository.GetUserDialogsAsync(login).Result;
            var dialog = dialogs.First();

            var filename = "beatles-avatar.jpg";
            var image = ImageProvider.GetAvatar(filename);

            var message = new MessageEntity()
            {
                Text = "aaaaa",
                Sender = login,
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
                UserLogin = login
            };
            var dialogMessages = _messageRepository.GetDialogMessagesAsync(dialogMessagesParameter).Result;


            var files = _messageRepository.GetMessagesContent(dialogMessages
                .SelectMany(x => x.Content.Select(y => y.FileId.ToString()))
                .ToArray());

            Assert.AreEqual(files.Count, 1);

            var file = files.First();
            Assert.AreEqual(file.FileBytes.Length, image.Length);
        }

        

    }
}
