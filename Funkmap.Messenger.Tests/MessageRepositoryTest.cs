using System.Linq;
using Autofac;
using Funkmap.Common.Cqrs.Abstract;
using Funkmap.Messenger.Command;
using Funkmap.Messenger.Command.Commands;
using Funkmap.Messenger.Entities;
using Funkmap.Messenger.Query;
using Funkmap.Messenger.Query.Queries;
using Funkmap.Messenger.Query.Responses;
using Funkmap.Messenger.Tests.Data;
using Funkmap.Middleware.Modules;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Funkmap.Messenger.Tests
{
    [TestClass]
    public class MessageRepositoryTest
    {
        private ICommandBus _commandBus;

        private IQueryContext _queryContext;

        [TestInitialize]
        public void Initialize()
        {

            var builder = new ContainerBuilder();
            new MessengerQueryModule().Register(builder);
            new MessengerCommandModule().Register(builder);
            new CqrsModule().Register(builder);

            var db = MessengerDbProvider.DropAndCreateDatabase;
            var messagesCollection = db.GetCollection<MessageEntity>(MessengerCollectionNameProvider.MessagesCollectionName);

            var container = builder.Build();

            _queryContext = container.Resolve<IQueryContext>();
            _commandBus = container.Resolve<ICommandBus>();

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

            var saveMessageCommand = new SaveMessageCommand()
            {
                Text = "aaaaa",
                Sender = login,
                DialogId = dialog.DialogId
            };

            _commandBus.Execute(saveMessageCommand).GetAwaiter().GetResult();

            var newDialogMessages = _queryContext.Execute<DialogMessagesQuery, DialogMessagesResponse>(query1).GetAwaiter().GetResult().Messages;

            Assert.AreNotEqual(dialogMessages.Count, newDialogMessages.Count);
            Assert.AreEqual(dialogMessages.Count + 1, newDialogMessages.Count);
        }

    }
}
