using System.Configuration;
using MongoDB.Driver;

namespace Funkmap.Tests.Data
{
    public class DbProvider
    {
        public static IMongoDatabase DropAndCreateDatabase
        {
            get
            {
                var connectionString = ConfigurationManager.ConnectionStrings["MongoDb"].ConnectionString;
                var databaseName = ConfigurationManager.AppSettings["MongoDbName"];
                var mongoClient = new MongoClient(connectionString);
                mongoClient.DropDatabase(databaseName);
                var db = mongoClient.GetDatabase(databaseName);
                new DataSeeder(db).SeedData();
                return db;
            }
        }
    }
}
