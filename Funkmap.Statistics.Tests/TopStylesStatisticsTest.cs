using System;
using System.Collections.Generic;
using Funkmap.Data.Entities;
using Funkmap.Data.Repositories;
using Funkmap.Statistics.Data.Entities;
using Funkmap.Statistics.Data.Repositories;
using Funkmap.Statistics.Data.Repositories.Abstract;
using Funkmap.Statistics.Data.Services;
using Funkmap.Statistics.Tests.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Funkmap.Statistics.Tests
{
    [TestClass]
    public class TopStylesStatisticsTest
    {
        private TopStylesStatisticsRepository _repository;

        private MusicianRepository _profilesRepository;

        private StatisticsMerger _merger;

        [TestInitialize]
        public void Initialize()
        {
            var db = FunkmapStatisticsTestDbProvider.Database;
            db.DropCollection(StatisticsCollectionNameProvider.StatisticsCollectionName);
            
            var profilesCollection = db.GetCollection<MusicianEntity>(CollectionNameProvider.BaseCollectionName);
            var typeStatisticsCollection = db.GetCollection<TopStylesStatisticsEntity>(StatisticsCollectionNameProvider.StatisticsCollectionName);

            _repository = new TopStylesStatisticsRepository(typeStatisticsCollection, profilesCollection);

            _profilesRepository = new MusicianRepository(profilesCollection);

            var statisticsCollection = db.GetCollection<BaseStatisticsEntity>(StatisticsCollectionNameProvider.StatisticsCollectionName);

            var statisticsRepositories = new List<IStatisticsRepository>() { _repository };

            _merger = new StatisticsMerger(statisticsCollection, statisticsRepositories);
        }

        [TestMethod]
        public void GetFullStatisticsTest()
        {
            var fullStatistics = _repository.BuildFullStatisticsAsync().GetAwaiter().GetResult();
            Assert.IsNotNull(fullStatistics);


            var begin = DateTime.UtcNow.AddMonths(-12);
            var end = DateTime.UtcNow.AddMonths(-10);
            var periodStatistics = _repository.BuildStatisticsAsync(begin, end).GetAwaiter().GetResult();
            Assert.IsNotNull(periodStatistics);
        }
    }
}
