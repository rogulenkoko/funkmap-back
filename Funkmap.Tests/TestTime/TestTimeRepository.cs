using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Common;
using Autofac.Extras.Moq;
using Funkmap.Data.Entities;
using Funkmap.Data.Entities.Abstract;
using Funkmap.Data.Repositories;
using Funkmap.Data.Services;
using Funkmap.Data.Services.Abstract;
using Funkmap.Tests.Funkmap.BigData;
using Funkmap.Tests.Funkmap.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace Funkmap.Tests.TestTime
{
    [TestClass]
    public class TestTimeRepository
    {
        private TestBaseRepository _testBaseRepository;
        [TestInitialize]
        public void Init()
        {
            var filterServices = new List<IFilterService>() { new MusicianFilterService() };

            using (var mock = AutoMock.GetLoose())
            {
                var bucket = mock.Mock<IGridFSBucket>();
                IFilterFactory factory = new FilterFactory(filterServices);
                _testBaseRepository = new TestBaseRepository(
                    new BaseRepository(
                        FunkmapTestDbProvider
                            .DropAndCreateDatabase
                            .GetCollection<BaseEntity>(CollectionNameProvider.BaseCollectionName),
                        bucket.Object, factory));
            }

            
        }

        [TestMethod]
        public void MainTest()
        {
            _testBaseRepository.CheckIfLoginExistAsync(null).GetAwaiter();
            _testBaseRepository.GetAllAsyns().GetAwaiter();
            _testBaseRepository.GetFilteredAsync(null, null).GetAwaiter();
            _testBaseRepository.GetFullNearestAsync(null).GetAwaiter();
            _testBaseRepository.UpdateAsync(null).GetAwaiter();
            _testBaseRepository.GetAllFilteredLoginsAsync(null,null).GetAwaiter();
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
