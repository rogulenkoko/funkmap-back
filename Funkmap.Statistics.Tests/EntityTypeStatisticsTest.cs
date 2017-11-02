using System;
using System.Linq;
using Autofac.Extras.Moq;
using Funkmap.Data.Entities;
using Funkmap.Data.Entities.Abstract;
using Funkmap.Data.Repositories;
using Funkmap.Data.Services.Abstract;
using Funkmap.Statistics.Data.Entities;
using Funkmap.Statistics.Data.Repositories;
using Funkmap.Statistics.Tests.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver.GridFS;

namespace Funkmap.Statistics.Tests
{
    [TestClass]
    public class EntityTypeStatisticsTest
    {

        private EntityTypeStatisticsRepository _repository;

        private BaseRepository _profilesRepository;


        [TestInitialize]
        public void Initialize()
        {
            var db = FunkmapStatisticsTestDbProvider.Database;
            db.DropCollection(CollectionNameProvider.StatisticsCollectionName);


            var profilesCollection = db.GetCollection<BaseEntity>(CollectionNameProvider.BaseCollectionName);
            _repository = new EntityTypeStatisticsRepository(db.GetCollection<EntityTypeStatisticsEntity>(CollectionNameProvider.StatisticsCollectionName), profilesCollection);

            using (var mock = AutoMock.GetLoose())
            {
                var gridFs = mock.Mock<IGridFSBucket>();
                var filterFactory = mock.Mock<IFilterFactory>();

                _profilesRepository = new BaseRepository(profilesCollection, gridFs.Object, filterFactory.Object);
            }

            
        }

        [TestMethod]
        public void GetFullStatistics()
        {
            var fullStatistics = _repository.BuildFullStatisticsAsync().GetAwaiter().GetResult();
            _repository.UpdateAsync(fullStatistics).GetAwaiter().GetResult();

            var savedStatistic = _repository.GetAsync(fullStatistics.Id.ToString()).GetAwaiter().GetResult();

            Assert.IsNotNull(savedStatistic);

            Assert.IsTrue(CompareStatistics(savedStatistic, fullStatistics));

            var begin = DateTime.UtcNow.AddMonths(-8);
            var end = DateTime.UtcNow.AddMonths(-3);
            var periodStatistics = _repository.BuildStatisticsAsync(begin, end).GetAwaiter().GetResult();
            Assert.IsNotNull(periodStatistics);

            _repository.MergeStatistics().GetAwaiter().GetResult();
            var mergedStatistics = _repository.GetAsync(fullStatistics.Id.ToString()).GetAwaiter().GetResult();

            Assert.IsTrue(CompareStatistics(mergedStatistics, fullStatistics));


            var newEntity = new BandEntity()
            {
                Login = Guid.NewGuid().ToString(),
                Name = "qweqwe",
                CreationDate = DateTime.UtcNow
            };
            _profilesRepository.CreateAsync(newEntity).GetAwaiter().GetResult();

            _repository.MergeStatistics().GetAwaiter().GetResult();
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
