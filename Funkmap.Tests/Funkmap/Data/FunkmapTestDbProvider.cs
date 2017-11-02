using System.Collections.Generic;
using System.Configuration;
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
    }
}
