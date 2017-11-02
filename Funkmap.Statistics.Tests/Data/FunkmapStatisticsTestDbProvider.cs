using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Funkmap.Statistics.Tests.Data
{
    public class FunkmapStatisticsTestDbProvider
    {
        public static IMongoDatabase Database
        {
            get
            {
                var connectionString = ConfigurationManager.ConnectionStrings["FunkmapMongoConnection"].ConnectionString;
                var databaseName = ConfigurationManager.AppSettings["FunkmapDbName"];
                var mongoClient = new MongoClient(connectionString);
                var db = mongoClient.GetDatabase(databaseName);
                return db;
            }
        }
    }
}
