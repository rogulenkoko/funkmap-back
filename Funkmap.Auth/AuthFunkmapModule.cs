using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using Autofac;
using Autofac.Features.AttributeFilters;
using Autofac.Integration.WebApi;
using Funkmap.Auth.Data;
using Funkmap.Auth.Data.Abstract;
using Funkmap.Auth.Data.Entities;
using Funkmap.Common.Abstract;
using Funkmap.Common.Azure;
using Funkmap.Common.Data.Mongo;
using Funkmap.Module.Auth.Services;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace Funkmap.Module.Auth
{

    public class AuthFunkmapModule : IFunkmapModule
    {
        public void Register(ContainerBuilder builder)
        {

            var connectionString = ConfigurationManager.ConnectionStrings["FunkmapAuthMongoConnection"].ConnectionString;
            var databaseName = ConfigurationManager.AppSettings["FunkmapAuthDbName"];
            var mongoClient = new MongoClient(connectionString);

            var databaseIocName = "auth";

            builder.Register(x => mongoClient.GetDatabase(databaseName)).As<IMongoDatabase>().Named<IMongoDatabase>(databaseIocName).SingleInstance();
            
            var loginBaseIndexModel = new CreateIndexModel<UserEntity>(Builders<UserEntity>.IndexKeys.Ascending(x => x.Login), new CreateIndexOptions() { Unique = true });

            builder.Register(container => container.ResolveNamed<IMongoDatabase>(databaseIocName).GetCollection<UserEntity>(AuthCollectionNameProvider.UsersCollectionName))
                .OnActivating(async collection => await collection.Instance.Indexes
                    .CreateManyAsync(new List<CreateIndexModel<UserEntity>>() { loginBaseIndexModel}))
                .As<IMongoCollection<UserEntity>>();


            //builder.Register(container =>
            //{
            //    var database = container.ResolveNamed<IMongoDatabase>(databaseIocName);
            //    var gridFs = new GridFSBucket(database);
            //    return new GridFsFileStorage(gridFs);
            //}).Named<GridFsFileStorage>(AuthCollectionNameProvider.AuthStorageName);

            //builder.Register(context => context.ResolveKeyed<GridFsFileStorage>(AuthCollectionNameProvider.AuthStorageName))
            //    .Keyed<IFileStorage>(AuthCollectionNameProvider.AuthStorageName)
            //    .InstancePerDependency();


            builder.Register(container =>
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("azureStorage"));
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                return new AzureFileStorage(blobClient, AuthCollectionNameProvider.AuthStorageName);
            }).Keyed<AzureFileStorage>(AuthCollectionNameProvider.AuthStorageName).InstancePerDependency();

            builder.Register(context => context.ResolveKeyed<AzureFileStorage>(AuthCollectionNameProvider.AuthStorageName))
                .Keyed<IFileStorage>(AuthCollectionNameProvider.AuthStorageName)
                .InstancePerDependency();

            builder.RegisterType<RegistrationContextManager>().As<IRegistrationContextManager>().SingleInstance();

            builder.RegisterType<AuthRepository>().As<IAuthRepository>().WithAttributeFiltering();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            Console.WriteLine("Загружен модуль авторизации");
        }
    }
}
