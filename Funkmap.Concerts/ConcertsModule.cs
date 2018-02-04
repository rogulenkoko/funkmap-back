using System;
using System.Configuration;
using Akka.Actor;
using Autofac;
using Funkmap.Common.Abstract;
using Funkmap.Common.Azure;
using Funkmap.Concerts.Actors;
using Funkmap.Concerts.Command;
using Funkmap.Concerts.Entities;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using MongoDB.Driver;

namespace Funkmap.Concerts
{
    public class ConcertsModule : IFunkmapModule
    {
        public void Register(ContainerBuilder builder)
        {

            builder.RegisterType<FinishActor>(); 
            builder.RegisterType<SchedulerActor>();
            builder.RegisterType<ActivationActor>();

            var system = ActorSystem.Create("ConcertsSystem");
            builder.RegisterInstance(system).As<ActorSystem>();


            var connectionString = ConfigurationManager.ConnectionStrings["FunkmapMessengerMongoConnection"].ConnectionString;
            var databaseName = ConfigurationManager.AppSettings["FunkmapMessengerDbName"];
            var mongoClient = new MongoClient(connectionString);

            var databaseIocName = "messenger";

            builder.Register(x => mongoClient.GetDatabase(databaseName)).As<IMongoDatabase>().Named<IMongoDatabase>(databaseIocName).SingleInstance();

            builder.Register(container => container.ResolveNamed<IMongoDatabase>(databaseIocName).GetCollection<ConcertEntity>(ConcertsCollectionNameProvider.CollectionName))
                .As<IMongoCollection<ConcertEntity>>();

            builder.Register(container =>
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("azureStorage"));
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                return new AzureFileStorage(blobClient, ConcertsCollectionNameProvider.StorageName);
            }).Keyed<AzureFileStorage>(ConcertsCollectionNameProvider.StorageName).SingleInstance();

            builder.Register(context => context.ResolveKeyed<AzureFileStorage>(ConcertsCollectionNameProvider.StorageName))
                .Keyed<IFileStorage>(ConcertsCollectionNameProvider.StorageName)
                .SingleInstance();

            Console.WriteLine("Загружен модуль концертов");
        }
    }
}
