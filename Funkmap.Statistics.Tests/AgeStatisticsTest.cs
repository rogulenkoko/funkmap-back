using System;
using System.Collections.Generic;
using System.Linq;
using Funkmap.Common.Abstract;
using Funkmap.Common.Cqrs.Abstract;
using Funkmap.Data;
using Funkmap.Data.Entities.Entities;
using Funkmap.Data.Entities.Entities.Abstract;
using Funkmap.Data.Repositories;
using Funkmap.Data.Services.Abstract;
using Funkmap.Domain.Abstract.Repositories;
using Funkmap.Domain.Models;
using Funkmap.Domain.Parameters;
using Funkmap.Statistics.Data.Entities;
using Funkmap.Statistics.Data.Repositories;
using Funkmap.Statistics.Data.Repositories.Abstract;
using Funkmap.Statistics.Data.Services;
using Funkmap.Statistics.Tests.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Funkmap.Statistics.Tests
{
    [TestClass]
    public class AgeStatisticsTest
    {
        private AgeStatisticsRepository _repository;

        private MusicianRepository _profilesRepository;

        private IBaseCommandRepository _commandRepository;

        private StatisticsMerger _merger;


        [TestInitialize]
        public void Initialize()
        {
            var db = FunkmapStatisticsTestDbProvider.Database;
            db.DropCollection(StatisticsCollectionNameProvider.StatisticsCollectionName);

            var collection = db.GetCollection<BaseEntity>(CollectionNameProvider.BaseCollectionName);
            var profilesCollection = db.GetCollection<MusicianEntity>(CollectionNameProvider.BaseCollectionName);
           
            var typeStatisticsCollection = db.GetCollection<AgeStatisticsEntity>(StatisticsCollectionNameProvider.StatisticsCollectionName);

            var ageInfoProvider= new AgeInfoProvider();
            _repository = new AgeStatisticsRepository(typeStatisticsCollection, profilesCollection, ageInfoProvider);

            var storage = new Mock<IFileStorage>().Object;
            var eventBus = new Mock<IEventBus>().Object;
            _commandRepository = new BaseCommandRepository(collection, storage, new List<IUpdateDefenitionBuilder>(), eventBus);


            var statisticsCollection = db.GetCollection<BaseStatisticsEntity>(StatisticsCollectionNameProvider.StatisticsCollectionName);

            var statisticsRepositories = new List<IStatisticsRepository>() { _repository };

            _merger = new StatisticsMerger(statisticsCollection, statisticsRepositories);



        }

        [TestMethod]
        public void GetFullStatistics()
        {
            var fullStatistics = _repository.BuildFullStatisticsAsync().GetAwaiter().GetResult() as AgeStatisticsEntity;
            _repository.UpdateAsync(fullStatistics).GetAwaiter().GetResult();

            var savedStatistic = _repository.GetAsync(fullStatistics.Id.ToString()).GetAwaiter().GetResult();

            Assert.IsNotNull(savedStatistic);

            Assert.IsTrue(CompareStatistics(savedStatistic, fullStatistics));

            var begin = new DateTime(2016, 11, 9);
            var end = new DateTime(2017, 11, 9);
            var periodStatistics = _repository.BuildStatisticsAsync(begin, end).GetAwaiter().GetResult();
            Assert.IsNotNull(periodStatistics);

            _merger.MergeStatistics().GetAwaiter().GetResult();
            var mergedStatistics = _repository.GetAsync(fullStatistics.Id.ToString()).GetAwaiter().GetResult();

            Assert.IsTrue(CompareStatistics(mergedStatistics, fullStatistics));


            var newEntity = new Musician()
            {
                BirthDate = new DateTime(1980,11,11),
                Login = Guid.NewGuid().ToString(),
                Name = "qweqwe"
            };

            _commandRepository.CreateAsync(new CommandParameter<Profile>() {Parameter = newEntity, UserLogin = "rogulenkoko"}).GetAwaiter().GetResult();

            _merger.MergeStatistics().GetAwaiter().GetResult();
            var newMergedStatistics = _repository.GetAsync(fullStatistics.Id.ToString()).GetAwaiter().GetResult();
            Assert.AreNotEqual(mergedStatistics, newMergedStatistics);


        }

        private bool CompareStatistics(AgeStatisticsEntity savedStatistic, AgeStatisticsEntity fullStatistics)
        {
            bool areEquals = true;
            foreach (var statistic in savedStatistic.CountStatistics)
            {
                var buildedStatistic = fullStatistics.CountStatistics.SingleOrDefault(x => x.Key == statistic.Key);

                if (buildedStatistic == null || buildedStatistic.Count != statistic.Count)
                {
                    areEquals = false;
                    break;
                }
            }
            return areEquals;
        }
    }
}
