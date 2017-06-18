using System.Configuration;
using MongoDB.Driver;

namespace Funkmap.Tests.Funkmap.Data
{
    public class FunkmapDbProvider
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
                new FunkmapDataSeeder(db).SeedData();
                return db;
            }
        }
    }
}
