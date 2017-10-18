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
using MongoDB.Driver.GridFS;

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

            builder.Register(container => container.ResolveNamed<IMongoDatabase>(databaseIocName).GetCollection<DialogEntity>(MessengerCollectionNameProvider.DialogsCollectionName))
                .As<IMongoCollection<DialogEntity>>();

            builder.Register(container => container.ResolveNamed<IMongoDatabase>(databaseIocName).GetCollection<MessageEntity>(MessengerCollectionNameProvider.MessagesCollectionName))
                .As<IMongoCollection<MessageEntity>>();

            var dialogLastMessageDateIndexModel = new CreateIndexModel<DialogEntity>(Builders<DialogEntity>.IndexKeys.Descending(x => x.LastMessageDate));
            var messageDialogIdIndexModel = new CreateIndexModel<MessageEntity>(Builders<MessageEntity>.IndexKeys.Ascending(x => x.DialogId));

            builder.Register(container =>
            {
                var database = container.Resolve<IMongoDatabase>();
                //database.CreateCollection("fs.files");
                //database.CreateCollection("fs.chunks");
                return new GridFSBucket(database);

            }).As<IGridFSBucket>();
            
            builder.RegisterType<DialogRepository>().As<IDialogRepository>();
            builder.RegisterType<MessageRepository>().As<IMessageRepository>();

            builder.RegisterBuildCallback(async c =>
            {
                //создание индексов при запуске приложения
                var dialogsCollection = c.Resolve<IMongoCollection<DialogEntity>>();
                await dialogsCollection.Indexes.CreateManyAsync(new List<CreateIndexModel<DialogEntity>>
                {
                    dialogLastMessageDateIndexModel,
                });


                var messagesCollection = c.Resolve<IMongoCollection<MessageEntity>>();
                await messagesCollection.Indexes.CreateManyAsync(new List<CreateIndexModel<MessageEntity>>
                {
                    messageDialogIdIndexModel,
                });
            });

            builder.RegisterType<MessengerConnectionService>().As<IMessengerConnectionService>().SingleInstance();

            builder.RegisterHubs(Assembly.GetExecutingAssembly());
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());


            Console.WriteLine("Загружен модуль мессенджера");
        }
    }
}
