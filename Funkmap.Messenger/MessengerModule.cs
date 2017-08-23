using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using Autofac;
using Autofac.Integration.SignalR;
using Autofac.Integration.WebApi;
using Funkmap.Auth.Contracts.Services;
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
            


            builder.Register(container => new GridFSBucket(container.Resolve<IMongoDatabase>())).As<IGridFSBucket>();
            
            builder.RegisterType<DialogRepository>().As<IDialogRepository>();

            builder.RegisterType<MessengerCacheService>().As<IMessengerCacheService>().SingleInstance();

            builder.RegisterType<UserService>().AsSelf();

            //раскоментить, когда модуль авторизации и месенджера будут под разными доменами
            //builder.RegisterType<UserService>().As<IUserMqService>();

            builder.RegisterHubs(Assembly.GetExecutingAssembly());
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());


            Console.WriteLine("Загружен модуль мессенджера");
        }
    }
}
