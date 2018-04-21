using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Autofac;
using Funkmap.Common.Abstract;
using Funkmap.Common.Cqrs;
using Funkmap.Common.Cqrs.Abstract;
using Funkmap.Common.Logger;
using Funkmap.Common.Tools;
using Funkmap.Messenger.Command;
using Funkmap.Messenger.Command.Commands;
using Funkmap.Messenger.Entities;
using Funkmap.Messenger.Query;
using Funkmap.Messenger.Query.Queries;
using Funkmap.Messenger.Query.Responses;
using Funkmap.Messenger.Tests.Data;
using Funkmap.Messenger.Tests.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;
using Moq;
using NLog;

namespace Funkmap.Messenger.Tests.Queries
{
    [TestClass]
    public class DialogsQueriesTest
    {

        private IQueryContext _queryContext;

        private ICommandBus _commandBus;

        private TestToolRepository _testToolRepository;

        [TestInitialize]
        public void Initialize()
        {
            var builder = new ContainerBuilder();
            
            builder.RegisterType<InMemoryEventBus>().As<IEventBus>().SingleInstance();
            builder.RegisterType<InMemoryCommandBus>().As<ICommandBus>();
            builder.RegisterType<CommandHandlerResolver>().As<ICommandHandlerResolver>();
            builder.RegisterType<QueryContext>().As<IQueryContext>();
            builder.RegisterType<InMemoryStorage>().As<IStorage>().SingleInstance();


            new MessengerQueryModule().Register(builder);
            new MessengerCommandModule().Register(builder);

            var db = MessengerDbProvider.DropAndCreateDatabase;

            var dialogsCollection = db.GetCollection<DialogEntity>(MessengerCollectionNameProvider.DialogsCollectionName);
            builder.RegisterInstance(dialogsCollection).As<IMongoCollection<DialogEntity>>();

            var messagesCollection = db.GetCollection<MessageEntity>(MessengerCollectionNameProvider.MessagesCollectionName);
            builder.RegisterInstance(messagesCollection).As<IMongoCollection<MessageEntity>>();

            
            var logger = new Mock<ILogger>().Object;
            builder.RegisterInstance(logger).As<ILogger>();
            builder.RegisterGeneric(typeof(FunkmapLogger<>)).As(typeof(IFunkmapLogger<>));
            var container = builder.Build();

            _queryContext = container.Resolve<IQueryContext>();
            _commandBus = container.Resolve<ICommandBus>();

            _testToolRepository = new TestToolRepository(dialogsCollection);
        }

        [TestMethod]
        public void GetUserDialogsTest()
        {
            var login = _testToolRepository.GetAnyDialog().GetAwaiter().GetResult().Participants.First();
            
            var query = new UserDialogsQuery(login);
            var queryResult = _queryContext.ExecuteAsync<UserDialogsQuery, UserDialogsResponse>(query).GetAwaiter().GetResult();

            Assert.IsTrue(queryResult.Success);
            Assert.AreNotEqual(queryResult.Dialogs.Count, 0);
            Assert.IsTrue(queryResult.Dialogs.All(x=>x.Participants.Contains(login)));
        }

        [TestMethod]
        public void GetDialogMessages()
        {
            var dialog = _testToolRepository.GetAnyDialog().GetAwaiter().GetResult();

            var query = new DialogMessagesQuery
            {
                DialogId = dialog.Id.ToString(),
                UserLogin = dialog.Participants.First(),
                Skip = 0,
                Take = 100
            };
            var queryResult = _queryContext.ExecuteAsync<DialogMessagesQuery, DialogMessagesResponse>(query).GetAwaiter().GetResult();
            Assert.IsTrue(queryResult.Success);
            Assert.AreNotEqual(queryResult.Messages.Count, 0);
            Assert.IsTrue(queryResult.Messages.Count <= query.Take);
            Assert.IsTrue(queryResult.Messages.All(x=>x.DialogId == query.DialogId));
        }

        [TestMethod]
        public void GetDialogsNewMessagesCount()
        {
            var dialog = _testToolRepository.GetAnyDialog().GetAwaiter().GetResult();
            var user = dialog.Participants.First();
            var query = new DialogsNewMessagesCountQuery(user);

            var result = _queryContext.ExecuteAsync<DialogsNewMessagesCountQuery, DialogsNewMessagesCountResponse>(query).GetAwaiter().GetResult();
            Assert.IsTrue(result.Success);
            Assert.AreNotEqual(result.CountResults.Count, 0);


            var userDialogsQuery = new UserDialogsQuery(user);
            var userDialogsQueryResult = _queryContext.ExecuteAsync<UserDialogsQuery, UserDialogsResponse>(userDialogsQuery).GetAwaiter().GetResult();

            var allMessages = new List<Message>();

            foreach (var userDialog in userDialogsQueryResult.Dialogs)
            {
                var dialogMessagesQuery = new DialogMessagesQuery
                {
                    DialogId = userDialog.DialogId,
                    UserLogin = user,
                    Skip = 0,
                    Take = Int32.MaxValue
                };
                var dialogMessagesQueryResult = _queryContext.ExecuteAsync<DialogMessagesQuery, DialogMessagesResponse>(dialogMessagesQuery)
                    .GetAwaiter().GetResult();
                allMessages.AddRange(dialogMessagesQueryResult.Messages);

            }

            Assert.AreEqual(allMessages.Where(x=>x.IsNew).GroupBy(x=>x.DialogId).Count(), result.CountResults.Count);
        }
    }
}
