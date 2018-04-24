using System.Configuration;
using MongoDB.Driver;

namespace Funkmap.Auth.Tests.Data
{
    public class AuthTestDbProvider
    {
        public static IMongoDatabase DropAndCreateDatabase
        {
            get
            {
                var connectionString = ConfigurationManager.ConnectionStrings["FunkmapAuthMongoConnection"].ConnectionString;
                var databaseName = ConfigurationManager.AppSettings["FunkmapAuthDbName"];
                var mongoClient = new MongoClient(connectionString);
                mongoClient.DropDatabase(databaseName);
                var db = mongoClient.GetDatabase(databaseName);
                new AuthDataSeeder(db).SeedData();
                return db;
            }
        }
    }
}
