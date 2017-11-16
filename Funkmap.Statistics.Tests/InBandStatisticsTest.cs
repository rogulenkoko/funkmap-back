using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    public class InBandStatisticsTest
    {

        private InBandStatisticsRepository _repository;

        private MusicianRepository _profilesRepository;
        private StatisticsMerger _merger;

        [TestInitialize]
        public void Initialize()
        {
            var db = FunkmapStatisticsTestDbProvider.Database;
            db.DropCollection(StatisticsCollectionNameProvider.StatisticsCollectionName);

            var profilesCollection = db.GetCollection<MusicianEntity>(CollectionNameProvider.BaseCollectionName);
            var typeStatisticsCollection = db.GetCollection<InBandStatisticsEntity>(StatisticsCollectionNameProvider.StatisticsCollectionName);
            _profilesRepository = new MusicianRepository(profilesCollection);
            _repository = new InBandStatisticsRepository(typeStatisticsCollection, profilesCollection);
            var statisticsCollection = db.GetCollection<BaseStatisticsEntity>(StatisticsCollectionNameProvider.StatisticsCollectionName);

            var statisticsRepositories = new List<IStatisticsRepository>() { _repository };
            _merger = new StatisticsMerger(statisticsCollection, statisticsRepositories);
        }

        [TestMethod]
        public void GetFullStatisticsTest()
        {
            var fullStatistics = _repository.BuildFullStatisticsAsync().GetAwaiter().GetResult() as InBandStatisticsEntity;
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


            var newEntity = new MusicianEntity()
            {
                Login = Guid.NewGuid().ToString(),
                Name = "qweqwe",
                CreationDate = DateTime.UtcNow
                //BandLogins = new List<string>()
            };
            _profilesRepository.CreateAsync(newEntity).GetAwaiter().GetResult();

            _merger.MergeStatistics().GetAwaiter().GetResult();
            var newMergedStatistics = _repository.GetAsync(fullStatistics.Id.ToString()).GetAwaiter().GetResult();
            Assert.AreNotEqual(mergedStatistics, newMergedStatistics);
        }
        private bool CompareStatistics(InBandStatisticsEntity savedStatistic, InBandStatisticsEntity fullStatistics)
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
