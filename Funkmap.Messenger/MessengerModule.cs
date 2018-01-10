using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using Autofac;
using Autofac.Features.AttributeFilters;
using Autofac.Integration.SignalR;
using Autofac.Integration.WebApi;
using Funkmap.Common.Abstract;
using Funkmap.Common.Azure;
using Funkmap.Common.Cqrs.Abstract;
using Funkmap.Common.Data.Mongo;
using Funkmap.Messenger.Data;
using Funkmap.Messenger.Data.Repositories;
using Funkmap.Messenger.Data.Repositories.Abstract;
using Funkmap.Messenger.Entities;
using Funkmap.Messenger.Events.Dialogs;
using Funkmap.Messenger.Events.Messages;
using Funkmap.Messenger.Handlers;
using Funkmap.Messenger.Services;
using Funkmap.Messenger.Services.Abstract;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
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
                var database = container.ResolveNamed<IMongoDatabase>(databaseIocName);
                //database.CreateCollection("fs.files");
                //database.CreateCollection("fs.chunks");
                return new GridFSBucket(database);

            }).As<IGridFSBucket>().Named<IGridFSBucket>(MessengerCollectionNameProvider.MessengerStorage);

            builder.Register(container =>
            {
                var gridFs = container.ResolveNamed<IGridFSBucket>(MessengerCollectionNameProvider.MessengerStorage);
                return new GridFsFileStorage(gridFs);
            }).Named<GridFsFileStorage>(MessengerCollectionNameProvider.MessengerStorage);
            builder.Register(context => context.ResolveNamed<GridFsFileStorage>(MessengerCollectionNameProvider.MessengerStorage)).As<IFileStorage>().InstancePerDependency();


            //builder.Register(container =>
            //{
            //    CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("azureStorage"));
            //    CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            //    return new AzureFileStorage(blobClient, MessengerCollectionNameProvider.MessengerStorage);
            //}).Keyed<AzureFileStorage>(MessengerCollectionNameProvider.MessengerStorage).InstancePerDependency();

            //builder.Register(context => context.ResolveKeyed<AzureFileStorage>(MessengerCollectionNameProvider.MessengerStorage))
            //    .Keyed<IFileStorage>(MessengerCollectionNameProvider.MessengerStorage)
            //    .InstancePerDependency();

            builder.RegisterType<DialogRepository>().As<IDialogRepository>();
            builder.RegisterType<MessageRepository>().As<IMessageRepository>().WithAttributeFiltering();

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

            builder.RegisterType<DialogLastMessageEventHandler>()
                .As<IEventHandler<MessageSavedCompleteEvent>>()
                .As<IEventHandler>()
                .OnActivated(x => x.Instance.InitHandlers())
                .AutoActivate();

            builder.RegisterType<SignalrEventHandler>()
                .As<IEventHandler<DialogUpdatedEvent>>()
                .As<IEventHandler<MessageSavedCompleteEvent>>()
                .As<IEventHandler>()
                .OnActivated(x => x.Instance.InitHandlers())
                .AutoActivate();

            

            builder.RegisterHubs(Assembly.GetExecutingAssembly());
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());


            Console.WriteLine("Загружен модуль мессенджера");
        }
    }
}
