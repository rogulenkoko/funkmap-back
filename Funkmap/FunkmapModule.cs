using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using Autofac;
using Autofac.Integration.WebApi;
using Funkmap.Common.Abstract;
using Funkmap.Data.Entities;
using Funkmap.Data.Entities.Abstract;
using Funkmap.Data.Repositories;
using Funkmap.Data.Repositories.Abstract;
using MongoDB.Driver;

namespace Funkmap
{
    public class FunkmapModule : IFunkmapModule
    {
        public void Register(ContainerBuilder builder)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["MongoDb"].ConnectionString;
            var databaseName = ConfigurationManager.AppSettings["MongoDbName"];
            var mongoClient = new MongoClient(connectionString);
            builder.Register(x => mongoClient.GetDatabase(databaseName)).As<IMongoDatabase>().SingleInstance();

            var loginBaseIndexModel = new CreateIndexModel<BaseEntity>(Builders<BaseEntity>.IndexKeys.Ascending(x => x.Login), new CreateIndexOptions() { Unique = true });
            var geoBaseIndexModel = new CreateIndexModel<BaseEntity>(Builders<BaseEntity>.IndexKeys.Geo2DSphere(x => x.Location));
            builder.Register(container=> container.Resolve<IMongoDatabase>().GetCollection<BaseEntity>(CollectionNameProvider.BaseCollectionName))
                .OnActivating(async collection=> await collection.Instance.Indexes.CreateManyAsync(new List<CreateIndexModel<BaseEntity>>() { loginBaseIndexModel, geoBaseIndexModel }))
                .As<IMongoCollection<BaseEntity>>();

            var loginMusicianIndexModel = new CreateIndexModel<MusicianEntity>(Builders<MusicianEntity>.IndexKeys.Ascending(x => x.Login), new CreateIndexOptions() { Unique = true });
            builder.Register(container => container.Resolve<IMongoDatabase>().GetCollection<MusicianEntity>(CollectionNameProvider.BaseCollectionName))
                .OnActivating(async collection => await collection.Instance.Indexes.CreateManyAsync(new List<CreateIndexModel<MusicianEntity>>() { loginMusicianIndexModel }))
                .As<IMongoCollection<MusicianEntity>>();

            var loginBandIndexModel = new CreateIndexModel<BandEntity>(Builders<BandEntity>.IndexKeys.Ascending(x => x.Login), new CreateIndexOptions() { Unique = true });
            builder.Register(container => container.Resolve<IMongoDatabase>().GetCollection<BandEntity>(CollectionNameProvider.BaseCollectionName))
                .OnActivating(async collection => await collection.Instance.Indexes.CreateManyAsync(new List<CreateIndexModel<BandEntity>>() { loginBandIndexModel }))
                .As<IMongoCollection<BandEntity>>();



            builder.RegisterType<BaseRepository>().As<IBaseRepository>().SingleInstance();
            builder.RegisterType<MusicianRepository>().As<IMusicianRepository>().SingleInstance();
            builder.RegisterType<BandRepository>().As<IBandRepository>().SingleInstance();
            builder.RegisterType<ShopRepository>().As<IShopRepository>().SingleInstance();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            Console.WriteLine("Загружен основной модуль");
        }
    }
}
