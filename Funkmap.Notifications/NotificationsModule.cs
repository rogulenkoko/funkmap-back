using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Integration.WebApi;
using Funkmap.Common.Abstract;
using Funkmap.Common.RedisMq;
using Funkmap.Contracts.Notifications;
using Funkmap.Notifications.Contracts;
using Funkmap.Notifications.Data.Entities;
using Funkmap.Notifications.Services;
using Funkmap.Notifications.Services.Abstract;
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

            var types = AppDomain.CurrentDomain.GetAssemblies()
                .Where(x => x.FullName.Contains("Funkmap"))
                .SelectMany(s => s.GetTypes())
                .Where(x => x.GetInterfaces().Contains(typeof(INotificationTypesPair)))
                .Distinct()
                .ToList();

            var instances = types.Select(x => Activator.CreateInstance(x) as INotificationTypesPair).ToList();
            foreach (var instance in instances)
            {
                var serviceType = typeof(NotificationsService<,>).MakeGenericType(new[] { instance.RequestType, instance.ResponseType});
                builder.RegisterType(serviceType).As<INotificationsService>().As<IRedisMqConsumer>();
            }

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            Console.WriteLine("Загружен модуль уведомлений");
        }
    }
}
