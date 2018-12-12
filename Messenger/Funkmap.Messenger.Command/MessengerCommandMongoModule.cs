using System;
using System.Collections.Generic;
using Autofac;
using Funkmap.Azure;
using Funkmap.Common;
using Funkmap.Common.Abstract;
using Funkmap.Common.Data.Mongo;
using Funkmap.Messenger.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace Funkmap.Messenger.Command
{
    public static class MessengerCommandMongoModule
    {
        public static void RegisterMessengerDataModule(ContainerBuilder builder, IConfiguration config)
        {
            //all dependencies are actual for query module

            var mongoClient = new MongoClient(config["Mongo:Connection"]);

            var databaseName = "messenger";

            builder.Register(x => mongoClient.GetDatabase(databaseName)).As<IMongoDatabase>().Named<IMongoDatabase>(databaseName).SingleInstance();

            builder.Register(container => container.ResolveNamed<IMongoDatabase>(databaseName).GetCollection<DialogEntity>(MessengerCollectionNameProvider.DialogsCollectionName))
                .As<IMongoCollection<DialogEntity>>();

            builder.Register(container => container.ResolveNamed<IMongoDatabase>(databaseName).GetCollection<MessageEntity>(MessengerCollectionNameProvider.MessagesCollectionName))
                .As<IMongoCollection<MessageEntity>>();

            //FileStorage
            StorageType storageType = (StorageType)Enum.Parse(typeof(StorageType), config["FileStorage:Type"]);

            switch (storageType)
            {
                case StorageType.Azure:
                    builder.Register(container =>
                    {
                        CloudStorageAccount storageAccount = CloudStorageAccount.Parse(config["FileStorage:Azure"]);
                        CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                        return new AzureFileStorage(blobClient, MessengerCollectionNameProvider.MessengerStorage);
                    }).Keyed<AzureFileStorage>(MessengerCollectionNameProvider.MessengerStorage).SingleInstance();

                    builder.Register(context => context.ResolveKeyed<AzureFileStorage>(MessengerCollectionNameProvider.MessengerStorage))
                        .Keyed<IFileStorage>(MessengerCollectionNameProvider.MessengerStorage)
                        .SingleInstance();
                    break;

                case StorageType.GridFs:
                    builder.Register(container =>
                    {
                        var database = container.ResolveNamed<IMongoDatabase>(databaseName);
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
                    break;
            }

            builder.RegisterBuildCallback(async c =>
            {
                var dialogParticipantsIndexModel = new CreateIndexModel<DialogEntity>(Builders<DialogEntity>.IndexKeys.Ascending(x => x.Participants));

                var messageDialogIdIndexModel = new CreateIndexModel<MessageEntity>(Builders<MessageEntity>.IndexKeys.Ascending(x => x.DialogId));

                //создание индексов при запуске приложения
                var dialogsCollection = c.Resolve<IMongoCollection<DialogEntity>>();
                await dialogsCollection.Indexes.CreateManyAsync(new List<CreateIndexModel<DialogEntity>>
                {
                    dialogParticipantsIndexModel
                });


                var messagesCollection = c.Resolve<IMongoCollection<MessageEntity>>();
                await messagesCollection.Indexes.CreateManyAsync(new List<CreateIndexModel<MessageEntity>>
                {
                    messageDialogIdIndexModel,
                });
            });
        }
    }
}
