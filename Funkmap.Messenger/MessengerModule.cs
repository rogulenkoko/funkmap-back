using System;
using System.Reflection;
using Autofac;
using Autofac.Integration.SignalR;
using Funkmap.Common.Abstract;
using Funkmap.Messenger.Data.Entities;
using Funkmap.Messenger.Data.Repositories;
using Funkmap.Messenger.Data.Repositories.Abstract;
using Funkmap.Messenger.Services;
using MongoDB.Driver;

namespace Funkmap.Messenger
{
    public class MessengerModule : IFunkmapModule
    {
        public void Register(ContainerBuilder builder)
        {
            builder.Register(container => container.Resolve<IMongoDatabase>().GetCollection<MessageEntity>(MessengerCollectionNameProvider.MessegesCollectionName))
                .As<IMongoCollection<MessageEntity>>();

            builder.Register(container => container.Resolve<IMongoDatabase>().GetCollection<DialogEntity>(MessengerCollectionNameProvider.DialogsCollectionName))
                .As<IMongoCollection<DialogEntity>>();

            builder.RegisterType<MessageRepository>().As<IMessageRepository>();
            builder.RegisterType<DialogRepository>().As<IDialogRepository>();

            builder.RegisterType<MessengerCacheService>().As<IMessengerCacheService>().SingleInstance();

            builder.RegisterHubs(Assembly.GetExecutingAssembly());

            Console.WriteLine("Загружен модуль мессенджера");
        }
    }
}
