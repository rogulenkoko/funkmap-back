using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Common.Data.Mongo.Abstract;
using Funkmap.Data.Entities;
using Funkmap.Data.Entities.Abstract;
using Funkmap.Statistics.Data.Entities;
using Funkmap.Statistics.Data.Repositories;
using Funkmap.Statistics.Data.Repositories.Abstract;
using Funkmap.Tests.Funkmap.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;

namespace Funkmap.Tests.Statistics
{
    [TestClass]
    public class StatisticsRepositoryTest
    {
        private IMongoDatabase _mongoDatabase;

        [TestInitialize]
        public void Initialize()
        {
            _mongoDatabase = FunkmapTestDbProvider.DropAndCreateDatabase;
            //_statisticsRepository = new TopEntityStatisticsRepository(db.GetCollection<TopEntityStatisticsEntity>(CollectionNameProvider.BaseCollectionName),
               // db.GetCollection<BaseEntity>(CollectionNameProvider.BaseCollectionName));
        }

        

        [TestMethod]
        public void SexStatisticTest()
        {//работает
            var _statisticsRepository = new SexStatisticsRepository(_mongoDatabase.GetCollection<SexStatisticsEntity>(CollectionNameProvider.BaseCollectionName),
                 _mongoDatabase.GetCollection<MusicianEntity>(CollectionNameProvider.BaseCollectionName)); 
            var tetst = _statisticsRepository.BuildFullStatisticsAsync().GetAwaiter().GetResult();
            Assert.IsNotNull(tetst);
        }

        [TestMethod]
        public void EntityTypeStatisticTest()
        {//радобает
            var _statisticsRepository = new EntityTypeStatisticsRepository(_mongoDatabase.GetCollection<EntityTypeStatisticsEntity>(CollectionNameProvider.BaseCollectionName),
                _mongoDatabase.GetCollection<BaseEntity>(CollectionNameProvider.BaseCollectionName));
            var tetst = _statisticsRepository.BuildFullStatisticsAsync().GetAwaiter().GetResult();
            Assert.IsNotNull(tetst);
        }
        [TestMethod]
        public void TopEntityStatisticTest()
        {
            var _statisticsRepository = new TopEntityStatisticsRepository(_mongoDatabase.GetCollection<TopEntityStatisticsEntity>(CollectionNameProvider.BaseCollectionName),
                _mongoDatabase.GetCollection<BaseEntity>(CollectionNameProvider.BaseCollectionName));
            var tetst = _statisticsRepository.BuildFullStatisticsAsync().GetAwaiter().GetResult();
            Assert.IsNotNull(tetst);
        }
        [TestMethod]
        public void InstrumentStatisticTest()
        {// и эта
            var _statisticsRepository = new InstrumentStatisticsRepository(_mongoDatabase.GetCollection<InstrumentStatisticsEntity>(CollectionNameProvider.BaseCollectionName),
                _mongoDatabase.GetCollection<MusicianEntity>(CollectionNameProvider.BaseCollectionName));
            var tetst = _statisticsRepository.BuildFullStatisticsAsync().GetAwaiter().GetResult();
            Assert.IsNotNull(tetst);
        }
        /* [TestMethod]
         public void CityStatisticTest()
         {
             var _statisticsRepository = new CityStatisticsRepository(_mongoDatabase.GetCollection<CityeStatisticsEntity>(CollectionNameProvider.BaseCollectionName),
                 _mongoDatabase.GetCollection<BaseEntity>(CollectionNameProvider.BaseCollectionName));
             var tetst = _statisticsRepository.BuildFullStatisticsAsync().GetAwaiter().GetResult();
             Assert.IsNotNull(tetst);
         }*/
    }
}
