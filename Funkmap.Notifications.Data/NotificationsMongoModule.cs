using System.Configuration;
using Autofac;
using Funkmap.Common.Abstract;
using Funkmap.Notifications.Data.Entities;
using MongoDB.Driver;

namespace Funkmap.Notifications.Data
{
    public class NotificationsMongoModule : IFunkmapModule
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
        }
    }
}
