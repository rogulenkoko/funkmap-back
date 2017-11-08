using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Funkmap.Data.Entities;
using Funkmap.Data.Entities.Abstract;
using Funkmap.Data.Repositories;
using Funkmap.Data.Services.Abstract;
using Funkmap.Statistics.Data.Entities;
using Funkmap.Statistics.Data.Repositories;
using Funkmap.Statistics.Data.Repositories.Abstract;
using Funkmap.Statistics.Data.Services;
using Funkmap.Statistics.Tests.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver.GridFS;

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
            db.DropCollection(CollectionNameProvider.StatisticsCollectionName);
            
            var profilesCollection = db.GetCollection<MusicianEntity>(CollectionNameProvider.BaseCollectionName);
            var typeStatisticsCollection = db.GetCollection<TopStylesStatisticsEntity>(CollectionNameProvider.StatisticsCollectionName);

            _repository = new TopStylesStatisticsRepository(typeStatisticsCollection, profilesCollection);

            _profilesRepository = new MusicianRepository(profilesCollection);

            var statisticsCollection = db.GetCollection<BaseStatisticsEntity>(CollectionNameProvider.StatisticsCollectionName);

            var statisticsRepositories = new List<IStatisticsRepository>() { _repository };

            _merger = new StatisticsMerger(statisticsCollection, statisticsRepositories);
        }

        [TestMethod]
        public void GetFullStatisticsTest()
        {
            var fullStatistics = _repository.BuildFullStatisticsAsync().GetAwaiter().GetResult();
        }
    }
}
