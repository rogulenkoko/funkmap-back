using System.Configuration;
using Funkmap.Tests.Funkmap.Data;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace Funkmap.Tests.Messenger.Data
{
    public class MessengerDbProvider
    {
        public static IMongoDatabase DropAndCreateDatabase
        {
            get
            {
                var connectionString = ConfigurationManager.ConnectionStrings["FunkmapMessengerMongoConnection"].ConnectionString;
                var databaseName = ConfigurationManager.AppSettings["FunkmapMessengerDbName"];
                var mongoClient = new MongoClient(connectionString);
                mongoClient.DropDatabase(databaseName);
                var db = mongoClient.GetDatabase(databaseName);
                new MessengerDataSeeder(db).SeedData();
                return db;
            }
        }

        public static IGridFSBucket GetGridFsBucket(IMongoDatabase database)
        {
            database.CreateCollection("fs.files");
            database.CreateCollection("fs.chunks");
            return new GridFSBucket(database);
        }
    }
}
