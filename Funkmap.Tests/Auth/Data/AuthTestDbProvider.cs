using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Tests.Funkmap.Data;
using MongoDB.Driver;

namespace Funkmap.Tests.Auth.Data
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
