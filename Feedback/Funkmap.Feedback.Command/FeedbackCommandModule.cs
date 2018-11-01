using System;
using Autofac;
using Autofac.Features.AttributeFilters;
using Funkmap.Azure;
using Funkmap.Common.Abstract;
using Funkmap.Common.Data.Mongo;
using Funkmap.Cqrs.Abstract;
using Funkmap.Feedback.Command.CommandHandler;
using Funkmap.Feedback.Command.Commands;
using Funkmap.Feedback.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace Funkmap.Feedback.Command
{
    public static class FeedbackCommandModule
    {
        public static void RegisterFeedbackCommandModule(this ContainerBuilder builder, IConfiguration config)
        {
            builder.RegisterType<FeedbackCommandHandler>().As<ICommandHandler<FeedbackCommand>>()
                .WithAttributeFiltering();
            
            var mongoClient = new MongoClient(config["Mongo:Connection"]);

            const string databaseName = "feedback";

            builder.Register(x => mongoClient.GetDatabase(databaseName)).As<IMongoDatabase>().Named<IMongoDatabase>(databaseName).SingleInstance();

            builder.Register(container => container.ResolveNamed<IMongoDatabase>(databaseName).GetCollection<FeedbackEntity>(FeedbackCollectionNameProvider.FeedbackCollectionName))
                .As<IMongoCollection<FeedbackEntity>>();

            var storageType = (FileStorageType) Enum.Parse(typeof(FileStorageType), config["FileStorage:Type"]);

            switch (storageType)
            {
                case FileStorageType.AzureStorage:
                    builder.Register(container =>
                    {
                        var storageAccount = CloudStorageAccount.Parse(config["FileStorage:Azure"]);
                        var blobClient = storageAccount.CreateCloudBlobClient();
                        return new AzureFileStorage(blobClient, FeedbackCollectionNameProvider.FeedbackStorage);
                    }).Keyed<AzureFileStorage>(FeedbackCollectionNameProvider.FeedbackStorage).SingleInstance();

                    builder.Register(context =>
                            context.ResolveKeyed<AzureFileStorage>(FeedbackCollectionNameProvider.FeedbackStorage))
                        .Keyed<IFileStorage>(FeedbackCollectionNameProvider.FeedbackStorage)
                        .SingleInstance();
                    break;
                
                case FileStorageType.GridFs:
                    
                    builder.Register(container =>
                    {
                        var database = container.ResolveNamed<IMongoDatabase>(databaseName);
                        var gridFs = new GridFSBucket(database);
                        return new GridFsFileStorage(gridFs);
                    }).Keyed<GridFsFileStorage>(FeedbackCollectionNameProvider.FeedbackCollectionName);

                    builder.Register(context => context.ResolveNamed<GridFsFileStorage>(FeedbackCollectionNameProvider.FeedbackCollectionName))
                        .Keyed<IFileStorage>(FeedbackCollectionNameProvider.FeedbackCollectionName)
                        .InstancePerDependency();
                    
                    break;
                
                default: throw new ArgumentOutOfRangeException("Invalid file storage type.");
            }
        }
    }
}