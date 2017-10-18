using System;
using System.Configuration;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Integration.SignalR;
using Autofac.Integration.WebApi;
using Funkmap.Common.Abstract;
using Funkmap.Common.Redis.Abstract;
using Funkmap.Notifications.Contracts;
using Funkmap.Notifications.Data;
using Funkmap.Notifications.Data.Abstract;
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

            builder.RegisterType<NotificationRepository>().As<INotificationRepository>();

            builder.RegisterType<NotificationsConnectionService>().As<INotificationsConnectionService>().SingleInstance();

            var types = AppDomain.CurrentDomain.GetAssemblies()
                .Where(x => x.FullName.Contains("Funkmap"))
                .SelectMany(s => s.GetTypes())
                .Where(x => x.GetInterfaces().Contains(typeof(INotificationTypes)))
                .Distinct()
                .ToList();

            var instances = types.Select(x => Activator.CreateInstance(x) as INotificationTypes).ToList();
            foreach (var instance in instances)
            {
                var serviceType = typeof(NotificationsService<,>).MakeGenericType(new[] { instance.RequestType, instance.ResponseType});
                builder.RegisterType(serviceType)
                    .As<INotificationsService>()
                    .As<IMessageHandler>()
                    .OnActivated(x=> (x.Instance as IMessageHandler).InitHandlers())
                    .AutoActivate()
                    .WithParameter(new TypedParameter(typeof(NotificationType), instance.NotificationType));
            }

            builder.RegisterHubs(Assembly.GetExecutingAssembly());
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            Console.WriteLine("Загружен модуль уведомлений");
        }
    }
}
