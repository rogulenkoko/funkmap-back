using System.Configuration;
using Autofac;
using Funkmap.Statistics.Data.Entities;
using MongoDB.Driver;

namespace Funkmap.Statistics
{
    public partial class StatisticsModule
    {
        public void RegisterMongo(ContainerBuilder builder)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["FunkmapMongoConnection"].ConnectionString;
            var databaseName = ConfigurationManager.AppSettings["FunkmapDbName"];
            var mongoClient = new MongoClient(connectionString);

            var databaseIocName = "funkmap_statistics";

            builder.Register(x => mongoClient.GetDatabase(databaseName)).As<IMongoDatabase>().Named<IMongoDatabase>(databaseIocName).SingleInstance();

            builder.Register(container => container.ResolveNamed<IMongoDatabase>(databaseIocName).GetCollection<BaseStatisticsEntity>(StatisticsCollectionNameProvider.StatisticsCollectionName))
                .As<IMongoCollection<BaseStatisticsEntity>>();

            builder.Register(container => container.ResolveNamed<IMongoDatabase>(databaseIocName).GetCollection<CityStatisticsEntity>(StatisticsCollectionNameProvider.StatisticsCollectionName))
                .As<IMongoCollection<CityStatisticsEntity>>();

            builder.Register(container => container.ResolveNamed<IMongoDatabase>(databaseIocName).GetCollection<EntityTypeStatisticsEntity>(StatisticsCollectionNameProvider.StatisticsCollectionName))
                .As<IMongoCollection<EntityTypeStatisticsEntity>>();

            builder.Register(container => container.ResolveNamed<IMongoDatabase>(databaseIocName).GetCollection<InBandStatisticsEntity>(StatisticsCollectionNameProvider.StatisticsCollectionName))
                .As<IMongoCollection<InBandStatisticsEntity>>();

            builder.Register(container => container.ResolveNamed<IMongoDatabase>(databaseIocName).GetCollection<InstrumentStatisticsEntity>(StatisticsCollectionNameProvider.StatisticsCollectionName))
                .As<IMongoCollection<InstrumentStatisticsEntity>>();

            builder.Register(container => container.ResolveNamed<IMongoDatabase>(databaseIocName).GetCollection<SexStatisticsEntity>(StatisticsCollectionNameProvider.StatisticsCollectionName))
                .As<IMongoCollection<SexStatisticsEntity>>();

            builder.Register(container => container.ResolveNamed<IMongoDatabase>(databaseIocName).GetCollection<TopProfileStatisticsEntity>(StatisticsCollectionNameProvider.StatisticsCollectionName))
                .As<IMongoCollection<TopProfileStatisticsEntity>>();

            builder.Register(container => container.ResolveNamed<IMongoDatabase>(databaseIocName).GetCollection<TopStylesStatisticsEntity>(StatisticsCollectionNameProvider.StatisticsCollectionName))
                .As<IMongoCollection<TopStylesStatisticsEntity>>();

            builder.Register(container => container.ResolveNamed<IMongoDatabase>(databaseIocName).GetCollection<AgeStatisticsEntity>(StatisticsCollectionNameProvider.StatisticsCollectionName))
                .As<IMongoCollection<AgeStatisticsEntity>>();
        }
    }
}
