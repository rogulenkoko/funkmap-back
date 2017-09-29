using System.Configuration;
using Funkmap.Tests.Funkmap.Data;
using MongoDB.Driver;

namespace Funkmap.Tests.Funkmap.Stress
{
    public class FunkmapStressTestDbProvider
    {
        public static IMongoDatabase DropAndCreateDatabase
        {
            get
            {
                var connectionString = ConfigurationManager.ConnectionStrings["FunkmapMongoConnection"].ConnectionString;
                var databaseName = ConfigurationManager.AppSettings["FunkmapDbNameStress"];
                var mongoClient = new MongoClient(connectionString);
                mongoClient.DropDatabase(databaseName);
                var db = mongoClient.GetDatabase(databaseName);
                new FunkmapStressRandomSeeder(db).SeedData();
                return db;
            }
        }
    }
}
