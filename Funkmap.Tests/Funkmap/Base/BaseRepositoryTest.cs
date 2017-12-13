using System.Collections.Generic;
using System.Linq;
using Funkmap.Common;
using Funkmap.Common.Data.Mongo;
using Funkmap.Data.Entities;
using Funkmap.Data.Entities.Abstract;
using Funkmap.Data.Parameters;
using Funkmap.Data.Repositories;
using Funkmap.Data.Repositories.Abstract;
using Funkmap.Data.Services;
using Funkmap.Data.Services.Abstract;
using Funkmap.Tests.Funkmap.Data;
using Funkmap.Tests.Funkmap.Stress;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Funkmap.Tests.Funkmap.Base
{
    [TestClass]
    public class EntityRepositoryTest
    {

        private IBaseRepository _baseRepository;

        [TestInitialize]
        public void Initialize()
        {
            var filterServices = new List<IFilterService>() { new MusicianFilterService() };
            IFilterFactory factory = new FilterFactory(filterServices);
            var db = FunkmapTestDbProvider.DropAndCreateDatabase;

            var storage = new GridFsFileStorage(FunkmapTestDbProvider.GetGridFsBucket(db));

            _baseRepository = new BaseRepository(db.GetCollection<BaseEntity>(CollectionNameProvider.BaseCollectionName), storage, factory);
            //_baseRepository = new BaseRepository(FunkmapTestDbProvider.DropAndCreateStressDatabase.GetCollection<BaseEntity>(StatisticsCollectionNameProvider.BaseCollectionName), factory);

        }

        [TestMethod]
        public void GetAll()
        {
            var all = _baseRepository.GetAllAsyns().Result;
            Assert.AreEqual(all.Count, 15);
        }

        [TestMethod]
        public void GetNearestTest()
        {
            var parameter = new LocationParameter()
            {
                Longitude = 30.1,
                Latitude = 50.2,
                Take = 100,
                Skip = 0
            };
            var nearest = _baseRepository.GetNearestAsync(parameter).Result;
            Assert.AreEqual(nearest.Count, 100);
        }


        [TestMethod]
        public void GetSpecificTest()
        {
            var logins = new List<string>() { "razrab" };
            var result = _baseRepository.GetSpecificNavigationAsync(logins.ToArray()).Result;
            Assert.AreEqual(result.Count, 1);

            logins.Add("rogulenkoko");
            result = _baseRepository.GetSpecificNavigationAsync(logins.ToArray()).Result;
            Assert.AreEqual(result.Count, 2);
        }

        [TestMethod]
        public void GetFilteredMusicians()
        {
            var commonParameter = new CommonFilterParameter()
            {
                EntityType = EntityType.Musician,
                SearchText = "рог"
            };

            var musicianParameter = new MusicianFilterParameter()
            {
                Styles = new List<Styles>() { Styles.HipHop },
                Expirience = new List<ExpirienceType>() { ExpirienceType.Advanced }
            };

            var result = _baseRepository.GetFilteredAsync(commonParameter, musicianParameter).Result;
            Assert.AreEqual(result.Count, 1);
        }

        [TestMethod]
        public void UpdateEntity()
        {
            var entity = _baseRepository.GetAllAsyns().GetAwaiter().GetResult().First();
            (entity as MusicianEntity).Instrument = InstrumentType.Keyboard;
            _baseRepository.UpdateAsync(entity).GetAwaiter().GetResult();
            var updatedEntity = _baseRepository.GetSpecificNavigationAsync(new[] { entity.Login }).GetAwaiter().GetResult();
        }

        [TestMethod]
        public void GetUsersEntitiesCount()
        {
            var result = _baseRepository.GetUserEntitiesCountInfoAsync("rogulenkoko").GetAwaiter().GetResult();
            Assert.AreEqual(result.Count, 3);
        }
    }
}