using System.Collections.Generic;
using System.Linq;
using Autofac;
using Funkmap.Common.Cqrs.Abstract;
using Funkmap.Common.Data.Mongo;
using Funkmap.Messenger.Data;
using Funkmap.Messenger.Data.Parameters;
using Funkmap.Messenger.Data.Repositories;
using Funkmap.Messenger.Data.Repositories.Abstract;
using Funkmap.Messenger.Entities;
using Funkmap.Messenger.Query;
using Funkmap.Messenger.Query.Queries;
using Funkmap.Messenger.Query.Responses;
using Funkmap.Messenger.Tests.Data;
using Funkmap.Middleware.Modules;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;

namespace Funkmap.Messenger.Tests
{
    [TestClass]
    public class MessageRepositoryTest
    {
        private IMessageRepository _messageRepository;

        private IDialogRepository _dialogRepository;

        private IQueryContext _queryContext;

        [TestInitialize]
        public void Initialize()
        {

            var builder = new ContainerBuilder();
            new MessengerQueryModule().Register(builder);
            new CqrsModule().Register(builder);

            var db = MessengerDbProvider.DropAndCreateDatabase;
            var messagesCollection = db.GetCollection<MessageEntity>(MessengerCollectionNameProvider.MessagesCollectionName);

            _dialogRepository = new DialogRepository(db.GetCollection<DialogEntity>(MessengerCollectionNameProvider.DialogsCollectionName), messagesCollection);

            var fileStorage = new GridFsFileStorage(MessengerDbProvider.GetGridFsBucket(db));

            _messageRepository = new MessageRepository(messagesCollection, fileStorage);

            var container = builder.Build();

            _queryContext = container.Resolve<IQueryContext>();
        }

        [TestMethod]
        public void GetNewMessagesTest()
        {
            var login = "rogulenkoko";

            var query = new UserDialogsQuery(login);

            var dialogs = _queryContext.Execute<UserDialogsQuery, UserDialogsResponse>(query).GetAwaiter().GetResult().Dialogs;
            var param = new DialogsNewMessagesParameter()
            {
                Login = login,
                DialogIds = dialogs.Select(x=>x.DialogId).ToList()
            };
            var count = _messageRepository.GetDialogsWithNewMessagesAsync(param).GetAwaiter().GetResult();
            Assert.AreEqual(count.Count, 1);
        }

        [TestMethod]
        public void GetDialogMessages()
        {
            var login = "rogulenkoko";

            var query = new UserDialogsQuery(login);

            var dialog = _queryContext.Execute<UserDialogsQuery, UserDialogsResponse>(query).GetAwaiter().GetResult().Dialogs.First();

            var query1 = new DialogMessagesQuery()
            {
                DialogId = dialog.DialogId,
                Take = 4,
                Skip = 0,
                UserLogin = login
            };

            var messages = _queryContext.Execute<DialogMessagesQuery, DialogMessagesResponse>(query1).GetAwaiter().GetResult().Messages; 
            Assert.AreEqual(messages.Count, 4);

            var query2 = new DialogMessagesQuery()
            {
                DialogId = dialog.DialogId,
                Take = 4,
                Skip = 4,
                UserLogin = login
            };

            messages = _queryContext.Execute<DialogMessagesQuery, DialogMessagesResponse>(query2).GetAwaiter().GetResult().Messages;
            Assert.AreEqual(messages.Count, 2);

            var query3 = new DialogMessagesQuery()
            {
                DialogId = dialog.DialogId,
                Take = 5,
                Skip = 0,
                UserLogin = login
            };

            messages = _queryContext.Execute<DialogMessagesQuery, DialogMessagesResponse>(query3).GetAwaiter().GetResult().Messages;
            Assert.AreEqual(messages.Count, 5);

            var query4 = new DialogMessagesQuery()
            {
                DialogId = dialog.DialogId,
                Take = 5,
                Skip = 5,
                UserLogin = login
            };
            messages = _queryContext.Execute<DialogMessagesQuery, DialogMessagesResponse>(query4).GetAwaiter().GetResult().Messages;
            Assert.AreEqual(messages.Count, 1);
        }

        [TestMethod]
        public void CreateMessageTest()
        {
            var login = "rogulenkoko";


            var query = new UserDialogsQuery(login);

            var dialog = _queryContext.Execute<UserDialogsQuery, UserDialogsResponse>(query).GetAwaiter().GetResult().Dialogs.First();

            var query1 = new DialogMessagesQuery()
            {
                DialogId = dialog.DialogId,
                Skip = 0,
                Take = 100,
                UserLogin = login
            };
            var dialogMessages = _queryContext.Execute<DialogMessagesQuery, DialogMessagesResponse>(query1).GetAwaiter().GetResult().Messages;

            var message = new MessageEntity()
            {
                Text = "aaaaa",
                Sender = login,
                DialogId = new ObjectId(dialog.DialogId)
            };

            _messageRepository.AddMessage(message).Wait();

            var newDialogMessages = _queryContext.Execute<DialogMessagesQuery, DialogMessagesResponse>(query1).GetAwaiter().GetResult().Messages;

            Assert.AreNotEqual(dialogMessages.Count, newDialogMessages.Count);
            Assert.AreEqual(dialogMessages.Count + 1, newDialogMessages.Count);
        }

    }
}
