using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Common;
using Funkmap.Data.Entities;
using Funkmap.Data.Entities.Abstract;
using Funkmap.Data.Repositories;
using Funkmap.Data.Services;
using Funkmap.Data.Services.Abstract;
using Funkmap.Tests.Funkmap.BigData;
using Funkmap.Tests.Funkmap.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;

namespace Funkmap.Tests.TestTime
{
    [TestClass]
    public class TestTimeRepository
    {
        private TestBaseRepository _testBaseRepository;
        [TestInitialize]
        public void init()
        {
            var filterServices = new List<IFilterService>() { new MusicianFilterService() };
            IFilterFactory factory = new FilterFactory(filterServices);
            _testBaseRepository=new TestBaseRepository(
                new BaseRepository(
                    FunkmapTestDbProvider
                    .DropAndCreateDatabase
                    .GetCollection<BaseEntity>(CollectionNameProvider.BaseCollectionName),
                    factory));
        }

        [TestMethod]
        public void mainTest()
        {
            _testBaseRepository.TimeCheckIfLoginExist();
            _testBaseRepository.TimeGetAllFilteredLogins();
            _testBaseRepository.TimeGetFiltered();
            _testBaseRepository.TimeGetFullNearest();
            _testBaseRepository.TimeGetNearest();
            _testBaseRepository.TimeGetSpecific();
            _testBaseRepository.TimeGetUserEntitiesCountInfo();
            _testBaseRepository.TimeGetUserEntitiesLogins();
            _testBaseRepository.TimeUpdate();
            _testBaseRepository.TimeGetAll();
        }



        

        
    }
}
