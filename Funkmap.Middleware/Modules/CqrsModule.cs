using System;
using System.Configuration;
using Autofac;
using Funkmap.Common.Abstract;
using Funkmap.Common.Serialization;
using Funkmap.Common.Tools;
using Funkmap.Cqrs;
using Funkmap.Cqrs.Abstract;
using Funkmap.Middleware.Settings;
using Funkmap.Redis;
using Funkmap.Redis.Autofac;

namespace Funkmap.Middleware.Modules
{
    public class CqrsModule : IFunkmapModule
    {
        public void Register(ContainerBuilder builder)
        {

            builder.RegisterType<NewtonSerializer>().As<ISerializer>();

            MessageQueueType queueType;
            Enum.TryParse(ConfigurationManager.AppSettings["message-queue-type"], out queueType);

            switch (queueType)
            {
                case MessageQueueType.Memory:
                    builder.RegisterType<InMemoryEventBus>().As<IEventBus>().SingleInstance();
                    builder.RegisterType<InMemoryCommandBus>().As<ICommandBus>();
                    builder.RegisterType<CommandHandlerResolver>().As<ICommandHandlerResolver>();
                    break;
                case MessageQueueType.Redis:
                    builder.RegisterType<RedisEventBus>().As<IEventBus>().SingleInstance();
                    builder.RegisterModule<RedisModule>();
                    builder.RegisterType<InMemoryCommandBus>().As<ICommandBus>();
                    builder.RegisterType<CommandHandlerResolver>().As<ICommandHandlerResolver>();
                    break;
            }


            CacheStorageType storageType;
            Enum.TryParse(ConfigurationManager.AppSettings["cache-storage-type"], out storageType);

            switch (storageType)
            {
                case CacheStorageType.Memory:
                    builder.RegisterType<InMemoryStorage>().As<IStorage>().SingleInstance();
                    break;
                case CacheStorageType.Redis:
                    builder.RegisterType<RedisStorage>().As<IStorage>().SingleInstance();
                    builder.RegisterModule<RedisModule>();
                    break;
            }

            builder.RegisterType<QueryContext>().As<IQueryContext>();
        }
    }
}
