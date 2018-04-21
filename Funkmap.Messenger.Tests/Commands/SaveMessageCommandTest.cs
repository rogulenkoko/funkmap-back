using System;
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

namespace Funkmap.Messenger.Tests.Commands
{
    [TestClass]
    public class SaveMessageCommandTest
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
        public void SaveMessagePositiveTest()
        {
            var dialog = _testToolRepository.GetAnyDialogAsync().GetAwaiter().GetResult();

            var command = new SaveMessageCommand()
            {
                Sender = dialog.Participants.First(),
                DialogId = dialog.Id.ToString(),
                Text = Guid.NewGuid().ToString()
            };

            _commandBus.ExecuteAsync<SaveMessageCommand>(command).GetAwaiter().GetResult();

            var query = new DialogMessagesQuery()
            {
                DialogId = command.DialogId,
                UserLogin = command.Sender,
                Skip = 0,
                Take = 1
            };

            var queryResult = _queryContext.ExecuteAsync<DialogMessagesQuery, DialogMessagesResponse>(query).GetAwaiter()
                .GetResult();
            Assert.IsTrue(queryResult.Success);
            Assert.AreEqual(queryResult.Messages.Count, query.Take);
            Assert.AreEqual(command.Text, queryResult.Messages.Single().Text);
        }
    }
}
