using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Messenger.Data.Entities;
using Funkmap.Messenger.Data.Parameters;
using Funkmap.Messenger.Data.Repositories;
using Funkmap.Messenger.Data.Repositories.Abstract;
using Funkmap.Tests.Messenger.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Funkmap.Tests.Messenger
{
    [TestClass]
    public class MessageRepositoryTest
    {
        private IMessageRepository _messageRepository;

        [TestInitialize]
        public void Initialize()
        {
            _messageRepository = new MessageRepository(MessengerDbProvider.DropAndCreateDatabase.GetCollection<MessageEntity>(CollectionNameProvider.MessegesCollectionName));

        }


        [TestMethod]
        public void GetDialogMessages()
        {
            var parameter = new DialogParameter()
            {
                Members = new[] { "rogulenkoko", "test" },
                Skip = 0,
                Take = 100
            };

            var messages = _messageRepository.GetDilaogMessages(parameter).Result;
            Assert.AreEqual(messages.Count, 5);

            parameter.Members = new[] { "rogulenkoko", "rogulenkoko" };
            messages = _messageRepository.GetDilaogMessages(parameter).Result;
            Assert.AreEqual(messages.Count, 1);

        }
    }
}
