using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using Autofac;
using Autofac.Integration.SignalR;
using Autofac.Integration.WebApi;
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

            var connectionString = ConfigurationManager.ConnectionStrings["FunkmapMessengerMongoConnection"].ConnectionString;
            var databaseName = ConfigurationManager.AppSettings["FunkmapMessengerDbName"];
            var mongoClient = new MongoClient(connectionString);

            var databaseIocName = "messenger";

            builder.Register(x => mongoClient.GetDatabase(databaseName)).As<IMongoDatabase>().Named<IMongoDatabase>(databaseIocName).SingleInstance();


            builder.Register(container => container.ResolveNamed<IMongoDatabase>(databaseIocName).GetCollection<MessageEntity>(MessengerCollectionNameProvider.MessegesCollectionName))
                .As<IMongoCollection<MessageEntity>>();

            builder.Register(container => container.ResolveNamed<IMongoDatabase>(databaseIocName).GetCollection<DialogEntity>(MessengerCollectionNameProvider.DialogsCollectionName))
                .As<IMongoCollection<DialogEntity>>();

            var messageDateIndex = new CreateIndexModel<MessageEntity>(Builders<MessageEntity>.IndexKeys.Descending(x => x.DateTimeUtc));

            
            builder.RegisterBuildCallback(async c =>
            {
                var collection = c.Resolve<IMongoCollection<MessageEntity>>();
                await collection.Indexes.CreateManyAsync(new List<CreateIndexModel<MessageEntity>>
                {
                    messageDateIndex
                });
            });

            builder.RegisterType<MessageRepository>().As<IMessageRepository>();
            builder.RegisterType<DialogRepository>().As<IDialogRepository>();

            builder.RegisterType<MessengerCacheService>().As<IMessengerCacheService>().SingleInstance();

            builder.RegisterHubs(Assembly.GetExecutingAssembly());
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());


            Console.WriteLine("Загружен модуль мессенджера");
        }
    }
}
