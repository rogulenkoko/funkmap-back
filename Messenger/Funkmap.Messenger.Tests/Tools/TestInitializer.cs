using Autofac;
using Funkmap.Common.Abstract;
using Funkmap.Common.Tools;
using Funkmap.Cqrs;
using Funkmap.Cqrs.Abstract;
using Funkmap.Logger;
using Funkmap.Messenger.Command;
using Funkmap.Messenger.Entities;
using Funkmap.Messenger.Query;
using Funkmap.Messenger.Tests.Data;
using MongoDB.Driver;
using Moq;
using NLog;

namespace Funkmap.Messenger.Tests.Tools
{
    public class TestInitializer
    {
        public ContainerBuilder Initialize()
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
            return builder;
        }
    }
}
