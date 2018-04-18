using System.Collections.Generic;
using System.Configuration;
using Funkmap.Data;
using Funkmap.Data.Entities.Entities.Abstract;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace Funkmap.Tests.Data
{
    public class FunkmapTestDbProvider
    {
        public static IMongoDatabase DropAndCreateDatabase()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["FunkmapMongoConnection"].ConnectionString;
            var databaseName = ConfigurationManager.AppSettings["FunkmapDbName"];

            var stressMode = bool.Parse(ConfigurationManager.AppSettings["stress_mode"]);

            var mongoClient = new MongoClient(connectionString);
            mongoClient.DropDatabase(databaseName);
            var db = mongoClient.GetDatabase(databaseName);
            CreateIndexes(db);

            if (stressMode)
            {
                new FunkmapStressRandomSeeder(db).SeedData();
            }
            else
            {
                new FunkmapDataSeeder(db).SeedData();
            }

            return db;

        }

        private static void CreateIndexes(IMongoDatabase db)
        {
            var entityTypeBaseIndexModel = new CreateIndexModel<BaseEntity>(Builders<BaseEntity>.IndexKeys.Ascending(x => x.EntityType));
            var geoBaseIndexModel = new CreateIndexModel<BaseEntity>(Builders<BaseEntity>.IndexKeys.Geo2DSphere(x => x.Location));

            var collection = db.GetCollection<BaseEntity>(CollectionNameProvider.BaseCollectionName);
            collection.Indexes.CreateManyAsync(new List<CreateIndexModel<BaseEntity>>
            {
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
