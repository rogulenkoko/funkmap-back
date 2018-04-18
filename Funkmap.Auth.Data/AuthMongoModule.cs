using System;
using System.Collections.Generic;
using System.Configuration;
using Autofac;
using Autofac.Features.AttributeFilters;
using Funkmap.Auth.Data.Entities;
using Funkmap.Auth.Domain.Abstract;
using Funkmap.Common;
using Funkmap.Common.Abstract;
using Funkmap.Common.Azure;
using Funkmap.Common.Data.Mongo;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace Funkmap.Auth.Data
{
    public class AuthMongoModule : IFunkmapModule
    {
        public void Register(ContainerBuilder builder)
        {
            //MongoDB

            var connectionString = ConfigurationManager.ConnectionStrings["FunkmapAuthMongoConnection"].ConnectionString;
            var databaseName = ConfigurationManager.AppSettings["FunkmapAuthDbName"];
            var mongoClient = new MongoClient(connectionString);

            var databaseIocName = "auth";

            builder.Register(x => mongoClient.GetDatabase(databaseName)).As<IMongoDatabase>().Named<IMongoDatabase>(databaseIocName).SingleInstance();

            var emailIndexModel = new CreateIndexModel<UserEntity>(Builders<UserEntity>.IndexKeys.Ascending(x => x.Email), new CreateIndexOptions() { Unique = true });

            builder.Register(container => container.ResolveNamed<IMongoDatabase>(databaseIocName).GetCollection<UserEntity>(AuthCollectionNameProvider.UsersCollectionName))
                .OnActivating(async collection => await collection.Instance.Indexes
                    .CreateManyAsync(new List<CreateIndexModel<UserEntity>>() { emailIndexModel }))
                .As<IMongoCollection<UserEntity>>();


            //FileStorage
            StorageType storageType = (StorageType)Enum.Parse(typeof(StorageType), ConfigurationManager.AppSettings["file-storage"]);

            switch (storageType)
            {
                case StorageType.Azure:

                    builder.Register(container =>
                        {
                            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("azure-storage"));
                            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                            return new AzureFileStorage(blobClient, AuthCollectionNameProvider.AuthStorageName);
                        }).Keyed<AzureFileStorage>(AuthCollectionNameProvider.AuthStorageName).SingleInstance();

                    builder.Register(context => context.ResolveKeyed<AzureFileStorage>(AuthCollectionNameProvider.AuthStorageName))
                        .Keyed<IFileStorage>(AuthCollectionNameProvider.AuthStorageName)
                        .SingleInstance();

                    break;

                case StorageType.GridFs:
                    builder.Register(container =>
                    {
                        var database = container.ResolveNamed<IMongoDatabase>(databaseIocName);
                        var gridFs = new GridFSBucket(database);
                        return new GridFsFileStorage(gridFs);
                    }).Named<GridFsFileStorage>(AuthCollectionNameProvider.AuthStorageName);

                    builder.Register(context => context.ResolveKeyed<GridFsFileStorage>(AuthCollectionNameProvider.AuthStorageName))
                        .Keyed<IFileStorage>(AuthCollectionNameProvider.AuthStorageName)
                        .InstancePerDependency();
                    break;
            }

            //Repositories

            builder.RegisterType<AuthRepository>().As<IAuthRepository>().WithAttributeFiltering();

        }
    }
}
