using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Funkmap.Data.Caches;
using Funkmap.Data.Caches.Base;
using Funkmap.Data.Entities.Abstract;
using Funkmap.Data.Repositories;
using Funkmap.Data.Services;
using Funkmap.Data.Services.Abstract;
using Funkmap.Tests.Funkmap.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver.GridFS;
using ServiceStack.Redis;

namespace Funkmap.Tests.TestTime
{
    [TestClass]
    public class TestTimeCacheRepository
    {
        private TestBaseRepository _testBaseRepository;
        [TestInitialize]
        public void Init()
        {
            var redisHost = "localhost:6379";
            //IRedisClientsManager redisClientManager = new PooledRedisClientManager(redisHost);
            var redisClientManager = new PooledRedisClientManager();
            RedisClient redisClient = (RedisClient) redisClientManager.GetClient();
            redisClient.SendTimeout = 2000;

            using (var mock = AutoMock.GetLoose())
            {
                var bucket = mock.Mock<IGridFSBucket>();
                var favCacheService = mock.Mock<IFavoriteCacheService>();
                var filterCacheService = mock.Mock<IFilteredCacheService>();

                var filterServices = new List<IFilterService>() { new MusicianFilterService() };
                IFilterFactory factory = new FilterFactory(filterServices);

                var baseRepository = new BaseRepository(
                    FunkmapTestDbProvider.DropAndCreateDatabase.GetCollection<BaseEntity>(CollectionNameProvider.BaseCollectionName),
                    bucket.Object, factory);

                var baseCacheRepository = new BaseCacheRepository(favCacheService.Object, filterCacheService.Object, baseRepository);
                _testBaseRepository = new TestBaseRepository(baseCacheRepository);
            }
            
        }

        [TestMethod]
        public void MainTestCache()
        {
            _testBaseRepository.CheckIfLoginExistAsync(null).GetAwaiter();
            _testBaseRepository.GetAllAsyns().GetAwaiter();
            _testBaseRepository.GetFilteredAsync(null, null).GetAwaiter();
            _testBaseRepository.GetFullNearestAsync(null).GetAwaiter();
            _testBaseRepository.UpdateAsync(null).GetAwaiter();
            _testBaseRepository.GetAllFilteredLoginsAsync(null, null).GetAwaiter();
            _testBaseRepository.CreateAsync(null).GetAwaiter();
            _testBaseRepository.DeleteAsync(null).GetAwaiter();
            _testBaseRepository.GetAllAsync().GetAwaiter().GetResult();
            _testBaseRepository.GetAsync(null).GetAwaiter();
            _testBaseRepository.GetSpecificFullAsync(null).GetAwaiter();
            _testBaseRepository.GetSpecificNavigationAsync(null).GetAwaiter();
            _testBaseRepository.GetNearestAsync(null).GetAwaiter();

        }

        
    }
}
