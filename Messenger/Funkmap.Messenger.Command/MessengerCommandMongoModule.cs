using System;
using System.Collections.Generic;
using System.Configuration;
using Autofac;
using Funkmap.Common;
using Funkmap.Common.Abstract;
using Funkmap.Common.Azure;
using Funkmap.Common.Data.Mongo;
using Funkmap.Messenger.Entities;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace Funkmap.Messenger.Command
{
    public class MessengerCommandMongoModule : IFunkmapModule
    {
        public void Register(ContainerBuilder builder)
        {
            //all dependencies are actual for query module

            var connectionString = ConfigurationManager.ConnectionStrings["FunkmapMessengerMongoConnection"].ConnectionString;
            var databaseName = ConfigurationManager.AppSettings["FunkmapMessengerDbName"];
            var mongoClient = new MongoClient(connectionString);

            var databaseIocName = "messenger";

            builder.Register(x => mongoClient.GetDatabase(databaseName)).As<IMongoDatabase>().Named<IMongoDatabase>(databaseIocName).SingleInstance();

            builder.Register(container => container.ResolveNamed<IMongoDatabase>(databaseIocName).GetCollection<DialogEntity>(MessengerCollectionNameProvider.DialogsCollectionName))
                .As<IMongoCollection<DialogEntity>>();

            builder.Register(container => container.ResolveNamed<IMongoDatabase>(databaseIocName).GetCollection<MessageEntity>(MessengerCollectionNameProvider.MessagesCollectionName))
                .As<IMongoCollection<MessageEntity>>();

            //FileStorage
            StorageType storageType = (StorageType)Enum.Parse(typeof(StorageType), ConfigurationManager.AppSettings["file-storage"]);

            switch (storageType)
            {
                case StorageType.Azure:
                    builder.Register(container =>
                    {
                        CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("azure-storage"));
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
