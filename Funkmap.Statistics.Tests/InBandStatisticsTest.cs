using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Data.Entities;
using Funkmap.Data.Repositories;
using Funkmap.Statistics.Data.Entities;
using Funkmap.Statistics.Data.Repositories;
using Funkmap.Statistics.Tests.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Funkmap.Statistics.Tests
{
    [TestClass]
    public class InBandStatisticsTest
    {

        private InBandStatisticsRepository _repository;

        private MusicianRepository _profilesRepository;

        [TestInitialize]
        public void Initialize()
        {
            var db = FunkmapStatisticsTestDbProvider.Database;
            db.DropCollection(CollectionNameProvider.StatisticsCollectionName);

            var profilesCollection = db.GetCollection<MusicianEntity>(CollectionNameProvider.BaseCollectionName);
            var typeStatisticsCollection = db.GetCollection<InBandStatisticsEntity>(CollectionNameProvider.StatisticsCollectionName);

            _repository = new InBandStatisticsRepository(typeStatisticsCollection, profilesCollection);
        }

        [TestMethod]
        public void GetFullStatisticsTest()
        {
            var fullStatistics = _repository.BuildFullStatisticsAsync().GetAwaiter().GetResult();
        }
    }
}
