using System;
using System.Configuration;
using System.Reflection;
using Autofac;
using Autofac.Integration.SignalR;
using Autofac.Integration.WebApi;
using Funkmap.Common.Abstract;
using Funkmap.Common.Redis.Abstract;
using Funkmap.Notifications.Data;
using Funkmap.Notifications.Data.Abstract;
using Funkmap.Notifications.Data.Entities;
using Funkmap.Notifications.Services;
using Funkmap.Notifications.Services.Abstract;
using Funkmap.Notifications.Services.Specific;
using MongoDB.Driver;

namespace Funkmap.Notifications
{
    public class NotificationsModule : IFunkmapModule
    {
        public void Register(ContainerBuilder builder)
        {

            var connectionString = ConfigurationManager.ConnectionStrings["FunkmapNotificationsMongoConnection"].ConnectionString;
            var databaseName = ConfigurationManager.AppSettings["FunkmapNotificationsDbName"];
            var mongoClient = new MongoClient(connectionString);

            var databaseIocName = "notifications";

            builder.Register(x => mongoClient.GetDatabase(databaseName)).As<IMongoDatabase>().Named<IMongoDatabase>(databaseIocName).SingleInstance();
            
            builder.Register(container => container.ResolveNamed<IMongoDatabase>(databaseIocName).GetCollection<NotificationEntity>(NotificationsCollectionNameProvider.BaseCollectionName))
                .As<IMongoCollection<NotificationEntity>>();

            builder.RegisterType<NotificationRepository>().As<INotificationRepository>();
            builder.RegisterType<NotificationAnswerService>().As<INotificationAnswerService>();
            builder.RegisterType<NotificationsConnectionService>().As<INotificationsConnectionService>();

            builder.RegisterType<NotificationService>()
                .As<IMessageHandler>()
                .OnActivated(x => x.Instance.InitHandlers())
                .AutoActivate();

            builder.RegisterHubs(Assembly.GetExecutingAssembly());
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            Console.WriteLine("Загружен модуль уведомлений");
        }
    }
}
