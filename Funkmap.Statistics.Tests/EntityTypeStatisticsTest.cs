using System;
using System.Collections.Generic;
using System.Linq;
using Autofac.Extras.Moq;
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
using MongoDB.Driver.GridFS;
using Moq;

namespace Funkmap.Statistics.Tests
{
    [TestClass]
    public class EntityTypeStatisticsTest
    {

        private EntityTypeStatisticsRepository _repository;

        private IBaseCommandRepository _commandRepository;

        private StatisticsMerger _merger;


        [TestInitialize]
        public void Initialize()
        {
            var db = FunkmapStatisticsTestDbProvider.Database;
            db.DropCollection(StatisticsCollectionNameProvider.StatisticsCollectionName);


            var profilesCollection = db.GetCollection<BaseEntity>(CollectionNameProvider.BaseCollectionName);
            var typeStatisticsCollection = db.GetCollection<EntityTypeStatisticsEntity>(StatisticsCollectionNameProvider.StatisticsCollectionName);

            _repository = new EntityTypeStatisticsRepository(typeStatisticsCollection, profilesCollection);
            

            var storage = new Mock<IFileStorage>().Object;
            var eventBus = new Mock<IEventBus>().Object;

            _commandRepository = new BaseCommandRepository(profilesCollection, storage, new List<IUpdateDefenitionBuilder>(), eventBus);

            var statisticsCollection = db.GetCollection<BaseStatisticsEntity>(StatisticsCollectionNameProvider.StatisticsCollectionName);

            var statisticsRepositories = new List<IStatisticsRepository>() {_repository};

            _merger = new StatisticsMerger(statisticsCollection, statisticsRepositories);



        }

        [TestMethod]
        public void GetFullStatistics()
        {
            var fullStatistics = _repository.BuildFullStatisticsAsync().GetAwaiter().GetResult() as EntityTypeStatisticsEntity;
            _repository.UpdateAsync(fullStatistics).GetAwaiter().GetResult();

            var savedStatistic = _repository.GetAsync(fullStatistics.Id.ToString()).GetAwaiter().GetResult();

            Assert.IsNotNull(savedStatistic);

            Assert.IsTrue(CompareStatistics(savedStatistic, fullStatistics));

            var begin = DateTime.UtcNow.AddMonths(-8);
            var end = DateTime.UtcNow.AddMonths(-3);
            var periodStatistics = _repository.BuildStatisticsAsync(begin, end).GetAwaiter().GetResult();
            Assert.IsNotNull(periodStatistics);

            _merger.MergeStatistics().GetAwaiter().GetResult();
            var mergedStatistics = _repository.GetAsync(fullStatistics.Id.ToString()).GetAwaiter().GetResult();

            Assert.IsTrue(CompareStatistics(mergedStatistics, fullStatistics));


            var newEntity = new Band()
            {
                Login = Guid.NewGuid().ToString(),
                Name = "qweqwe"
            };
            _commandRepository.CreateAsync(new CommandParameter<Profile>() {Parameter = newEntity, UserLogin = "rogulenkoko"}).GetAwaiter().GetResult();

            _merger.MergeStatistics().GetAwaiter().GetResult();
            var newMergedStatistics = _repository.GetAsync(fullStatistics.Id.ToString()).GetAwaiter().GetResult();
            Assert.AreNotEqual(mergedStatistics, newMergedStatistics);


        }

        private bool CompareStatistics(EntityTypeStatisticsEntity savedStatistic, EntityTypeStatisticsEntity fullStatistics)
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
