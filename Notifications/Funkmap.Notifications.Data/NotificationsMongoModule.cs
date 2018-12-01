using Autofac;
using Funkmap.Notifications.Data.Entities;
using Funkmap.Notifications.Domain.Abstract;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Funkmap.Notifications.Data
{
    public static class NotificationsMongoModule
    {
        public static void RegisterNotificationDataModule(this ContainerBuilder builder, IConfiguration config)
        {
            var mongoClient = new MongoClient(config["Mongo:Connection"]);
            var databaseName = "notifications";

            builder.Register(x => mongoClient.GetDatabase(databaseName)).As<IMongoDatabase>().Named<IMongoDatabase>(databaseName).SingleInstance();

            builder.Register(container => container.ResolveNamed<IMongoDatabase>(databaseName).GetCollection<NotificationEntity>(NotificationsCollectionNameProvider.BaseCollectionName))
                .As<IMongoCollection<NotificationEntity>>();
            
            builder.RegisterType<NotificationRepository>().As<INotificationRepository>();
        }
    }
}
