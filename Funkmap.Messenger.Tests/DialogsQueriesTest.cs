using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Funkmap.Common.Cqrs.Abstract;
using Funkmap.Common.Data.Mongo;
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
    public class DialogsQueriesTest
    {

        private IQueryContext _queryContext;

        private ICommandBus _commandBus;

        [TestInitialize]
        public void Initialize()
        {
            var builder = new ContainerBuilder();
            new MessengerQueryModule().Register(builder);
            new MessengerCommandModule().Register(builder);
            new CqrsModule().Register(builder);

            var db = MessengerDbProvider.DropAndCreateDatabase;

            var messagesCollection = db.GetCollection<MessageEntity>(MessengerCollectionNameProvider.MessagesCollectionName);

            var fileStorage = new GridFsFileStorage(MessengerDbProvider.GetGridFsBucket(db));

            var container = builder.Build();

            _queryContext = container.Resolve<IQueryContext>();
            _commandBus = container.Resolve<ICommandBus>();
        }

        [TestMethod]
        public void GetUserDialogs()
        {
            var login = "rogulenkoko";

            var query = new UserDialogsQuery(login);

            var dialogs = _queryContext.Execute<UserDialogsQuery, UserDialogsResponse>(query).GetAwaiter().GetResult().Dialogs;
            Assert.AreEqual(dialogs.Count, 4);

            var query1 = new UserDialogsQuery(login);
            dialogs = _queryContext.Execute<UserDialogsQuery, UserDialogsResponse>(query1).GetAwaiter().GetResult().Dialogs;
            Assert.AreEqual(dialogs.Count, 2);
        }

        [TestMethod]
        public void UpdateLastMessageDateTest()
        {
            var login = "rogulenkoko";
            var query = new UserDialogsQuery(login);

            var dialog = _queryContext.Execute<UserDialogsQuery, UserDialogsResponse>(query).GetAwaiter().GetResult().Dialogs.Last();

            var command = new UpdateDialogLastMessageCommand(dialog.DialogId, DateTime.Now);

            _commandBus.Execute(command).GetAwaiter().GetResult();

            var query2 = new UserDialogsQuery(login);
            var updatedDialog = _queryContext.Execute<UserDialogsQuery, UserDialogsResponse>(query2).GetAwaiter().GetResult().Dialogs.Last();

            Assert.AreEqual(updatedDialog.DialogId, dialog.DialogId);
        }

        [TestMethod]
        public void CreateDialog()
        {

            var createCommand = new CreateDialogCommand()
            {
                Participants = new List<string>() { "qwert", "trewq" }
            };

            _commandBus.Execute(createCommand).GetAwaiter().GetResult();
        }
    }
}
