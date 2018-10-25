using System;
using System.Collections.Generic;
using System.Configuration;
using Autofac;
using Autofac.Features.AttributeFilters;
using Funkmap.Azure;
using Funkmap.Common;
using Funkmap.Common.Abstract;
using Funkmap.Common.Data.Mongo;
using Funkmap.Cqrs.Abstract;
using Funkmap.Data.Caches.Base;
using Funkmap.Data.Entities.Entities;
using Funkmap.Data.Entities.Entities.Abstract;
using Funkmap.Data.Repositories;
using Funkmap.Data.Repositories.Decorators;
using Funkmap.Data.Services;
using Funkmap.Data.Services.Abstract;
using Funkmap.Data.Services.Update;
using Funkmap.Domain.Abstract.Repositories;
using Funkmap.Domain.Events;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace Funkmap.Data
{
    public class FunkmapMongoModule : IFunkmapModule
    {
        public void Register(ContainerBuilder builder)
        {
            //MongoDB
            var connectionString = ConfigurationManager.ConnectionStrings["FunkmapMongoConnection"].ConnectionString;
            var databaseName = ConfigurationManager.AppSettings["FunkmapDbName"];
            var mongoClient = new MongoClient(connectionString);

            var databaseIocName = "funkmap";

            builder.Register(x => mongoClient.GetDatabase(databaseName)).As<IMongoDatabase>().Named<IMongoDatabase>(databaseIocName).SingleInstance();


            builder.Register(container => container.ResolveNamed<IMongoDatabase>(databaseIocName).GetCollection<BaseEntity>(CollectionNameProvider.BaseCollectionName))
                .As<IMongoCollection<BaseEntity>>();

            builder.Register(container => container.ResolveNamed<IMongoDatabase>(databaseIocName).GetCollection<MusicianEntity>(CollectionNameProvider.BaseCollectionName))
                .As<IMongoCollection<MusicianEntity>>();

            builder.Register(container => container.ResolveNamed<IMongoDatabase>(databaseIocName).GetCollection<BandEntity>(CollectionNameProvider.BaseCollectionName))
                .As<IMongoCollection<BandEntity>>();

            builder.Register(container => container.ResolveNamed<IMongoDatabase>(databaseIocName).GetCollection<ShopEntity>(CollectionNameProvider.BaseCollectionName))
                .As<IMongoCollection<ShopEntity>>();

            builder.Register(container => container.ResolveNamed<IMongoDatabase>(databaseIocName).GetCollection<RehearsalPointEntity>(CollectionNameProvider.BaseCollectionName))
                .As<IMongoCollection<RehearsalPointEntity>>();

            builder.Register(container => container.ResolveNamed<IMongoDatabase>(databaseIocName).GetCollection<StudioEntity>(CollectionNameProvider.BaseCollectionName))
                .As<IMongoCollection<StudioEntity>>();


            //MongoDb Indexes
            var entityTypeBaseIndexModel = new CreateIndexModel<BaseEntity>(Builders<BaseEntity>.IndexKeys.Ascending(x => x.EntityType));
            var geoBaseIndexModel = new CreateIndexModel<BaseEntity>(Builders<BaseEntity>.IndexKeys.Geo2DSphere(x => x.Location));


            builder.RegisterBuildCallback(async c =>
            {
                //создание индексов при запуске приложения
                var collection = c.Resolve<IMongoCollection<BaseEntity>>();
                await collection.Indexes.CreateManyAsync(new List<CreateIndexModel<BaseEntity>>
                {
                    entityTypeBaseIndexModel,
                    geoBaseIndexModel,
                });
            });

            //Repositories
            var baseRepositoryName = nameof(IBaseQueryRepository);

            builder.RegisterType<BaseQueryRepository>()
                .Named<IBaseQueryRepository>(baseRepositoryName).WithAttributeFiltering();

            builder.RegisterDecorator<IBaseQueryRepository>((container, inner) =>
            {
                var favoriteService = container.Resolve<IFavoriteCacheService>();
                return new BaseQueryCacheRepository(favoriteService, inner);
            }, fromKey: baseRepositoryName);

            builder.RegisterType<MusicianRepository>().As<IMusicianRepository>();
            builder.RegisterType<BandRepository>().As<IBandRepository>();


            var baseCommandRepositoryName = nameof(IBaseCommandRepository);
            builder.RegisterType<BaseCommandRepository>()
                .Named<IBaseCommandRepository>(baseCommandRepositoryName).WithAttributeFiltering();
            
            builder.RegisterDecorator<IBaseCommandRepository>((container, inner) =>
            {
                var collection = container.Resolve<IMongoCollection<BaseEntity>>();
                return new BaseCommandAuthRepository(collection, inner);
            }, fromKey: baseCommandRepositoryName);


            builder.RegisterType<BaseQueryRepository>().As<IBaseQueryRepository>().WithAttributeFiltering();

            builder.RegisterType<ProAccountRepository>().As<IProAccountRepository>();

            //Cache Services
            builder.RegisterType<FavoriteCacheService>().As<IFavoriteCacheService>();
            builder.RegisterType<FilteredCacheService>().As<IFilteredCacheService>();
            
            //Filter Tools
            builder.RegisterType<FilterFactory>().As<IFilterFactory>();
            builder.RegisterType<BandFilterService>().As<IFilterService>();
            builder.RegisterType<MusicianFilterService>().As<IFilterService>();

            builder.RegisterType<MusicianUpdateDefinitionBuilder>().As<IUpdateDefinitionBuilder>();
            builder.RegisterType<BandUpdateDefinitionBuilder>().As<IUpdateDefinitionBuilder>();
            builder.RegisterType<ShopUpdateDefinitionBuilder>().As<IUpdateDefinitionBuilder>();


            //Event handlers
            builder.RegisterType<BandDependenciesController>()
                .As<IEventHandler<ProfileDeletedEvent>>()
                .As<IEventHandler<ProfileUpdatedEvent>>()
                .As<IEventHandler>()
                .OnActivated(x => x.Instance.InitHandlers())
                .AutoActivate()
                .SingleInstance();

            //FileStorage
            StorageType storageType = (StorageType)Enum.Parse(typeof(StorageType), ConfigurationManager.AppSettings["file-storage"]);

            switch (storageType)
            {
                case StorageType.Azure:
                    builder.Register(container =>
                    {
                        CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("azure-storage"));
                        CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                        return new AzureFileStorage(blobClient, CollectionNameProvider.StorageName);
                    }).Keyed<IFileStorage>(CollectionNameProvider.StorageName).SingleInstance();
                    break;

                case StorageType.GridFs:

                    builder.Register(container =>
                    {
                        var database = container.ResolveNamed<IMongoDatabase>(databaseIocName);
                        var gridFs = new GridFSBucket(database);
                        return new GridFsFileStorage(gridFs);
                    }).Keyed<GridFsFileStorage>(CollectionNameProvider.StorageName);

                    builder.Register(context => context.ResolveNamed<GridFsFileStorage>(CollectionNameProvider.StorageName))
                        .Keyed<IFileStorage>(CollectionNameProvider.StorageName)
                        .InstancePerDependency();
                    break;

                default:
                    throw new ArgumentException("Invalid storage type in configuration file. For example: <add key=\"file-storage\" value=\"Azure\"></add>");
            }
        }
    }
}
