using System;
using System.Linq;
using Autofac;
using Funkmap.Common.Abstract;
using Funkmap.Common.Tools;
using Funkmap.Cqrs;
using Funkmap.Cqrs.Abstract;
using Funkmap.Logger;
using Funkmap.Messenger.Command;
using Funkmap.Messenger.Command.Commands;
using Funkmap.Messenger.Entities;
using Funkmap.Messenger.Query;
using Funkmap.Messenger.Query.Queries;
using Funkmap.Messenger.Query.Responses;
using Funkmap.Messenger.Tests.Data;
using Funkmap.Messenger.Tests.Tools;
using MongoDB.Driver;
using Moq;
using NLog;
using Xunit;

namespace Funkmap.Messenger.Tests.Commands
{
    public class SaveMessageCommandTest
    {
        private readonly IQueryContext _queryContext;
        private readonly ICommandBus _commandBus;
        private readonly TestToolRepository _testToolRepository;

        public SaveMessageCommandTest()
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

        [Fact]
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
            Assert.True(queryResult.Success);
            Assert.Equal(queryResult.Messages.Count, query.Take);
            Assert.Equal(command.Text, queryResult.Messages.Single().Text);
        }
    }
}
