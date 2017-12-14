using System.Collections.Generic;
using System.Configuration;
using Funkmap.Data;
using Funkmap.Data.Entities.Abstract;
using Funkmap.Tests.Funkmap.Stress;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace Funkmap.Tests.Funkmap.Data
{
    public class FunkmapTestDbProvider
    {
        public static IMongoDatabase DropAndCreateDatabase
        {
            get
            {
                var connectionString = ConfigurationManager.ConnectionStrings["FunkmapMongoConnection"].ConnectionString;
                var databaseName = ConfigurationManager.AppSettings["FunkmapDbName"];
                var mongoClient = new MongoClient(connectionString);
                mongoClient.DropDatabase(databaseName);
                var db = mongoClient.GetDatabase(databaseName);
                CreateIndexes(db);
                new FunkmapDataSeeder(db).SeedData();
                return db;
            }
        }

        public static IMongoDatabase DropAndCreateStressDatabase
        {
            get
            {
                var connectionString = ConfigurationManager.ConnectionStrings["FunkmapMongoConnection"].ConnectionString;
                var databaseName = ConfigurationManager.AppSettings["FunkmapDbName"];
                var mongoClient = new MongoClient(connectionString);
                mongoClient.DropDatabase(databaseName);
                var db = mongoClient.GetDatabase(databaseName);

                CreateIndexes(db);
                new FunkmapStressRandomSeeder(db).SeedData();
                return db;
            }
        }

        private static void CreateIndexes(IMongoDatabase db)
        {
            var loginBaseIndexModel = new CreateIndexModel<BaseEntity>(Builders<BaseEntity>.IndexKeys.Ascending(x => x.Login), new CreateIndexOptions() { Unique = true });
            var entityTypeBaseIndexModel = new CreateIndexModel<BaseEntity>(Builders<BaseEntity>.IndexKeys.Ascending(x => x.EntityType));
            var geoBaseIndexModel = new CreateIndexModel<BaseEntity>(Builders<BaseEntity>.IndexKeys.Geo2DSphere(x => x.Location));

            var collection = db.GetCollection<BaseEntity>(CollectionNameProvider.BaseCollectionName);
            collection.Indexes.CreateManyAsync(new List<CreateIndexModel<BaseEntity>>
            {
                loginBaseIndexModel,
                entityTypeBaseIndexModel,
                geoBaseIndexModel,
            }).GetAwaiter().GetResult();
        }

        public static IGridFSBucket GetGridFsBucket(IMongoDatabase database)
        {
            database.CreateCollection("fs.files");
            database.CreateCollection("fs.chunks");
            return new GridFSBucket(database);
        }
    }
}
