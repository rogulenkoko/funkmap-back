using System.Collections.Generic;
using System.Linq;
using Funkmap.Common;
using Funkmap.Data.Entities;
using Funkmap.Data.Entities.Abstract;
using Funkmap.Data.Parameters;
using Funkmap.Data.Repositories;
using Funkmap.Data.Repositories.Abstract;
using Funkmap.Data.Services;
using Funkmap.Data.Services.Abstract;
using Funkmap.Tests.Funkmap.Data;
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
            var filterServices = new List<IFilterService>() {new MusicianFilterService()};
            IFilterFactory factory = new FilterFactory(filterServices);
            _baseRepository = new BaseRepository(FunkmapTestDbProvider.DropAndCreateDatabase.GetCollection<BaseEntity>(CollectionNameProvider.BaseCollectionName), factory);

        }

        [TestMethod]
        public void GetAll()
        {
            var all = _baseRepository.GetAllAsyns().Result;
            Assert.AreEqual(all.Count, 14);
        }

        [TestMethod]
        public void GetNearestTest()
        {
            var parameter = new LocationParameter()
            {
                Longitude = 30,
                Latitude = 50,
                RadiusDeg = 2
            };
            var nearest = _baseRepository.GetNearestAsync(parameter).Result;
            Assert.AreEqual(nearest.Count, 5);
        }


        [TestMethod]
        public void GetSpecificTest()
        {
            var logins = new List<string>() { "razrab" };
            var result = _baseRepository.GetSpecificAsync(logins.ToArray()).Result;
            Assert.AreEqual(result.Count, 1);

            logins.Add("rogulenkoko");
            result = _baseRepository.GetSpecificAsync(logins.ToArray()).Result;
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
                Styles = new List<Styles>() { Styles.HipHop},
                Expirience = new List<ExpirienceType>() { ExpirienceType.Advanced}
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
            var updatedEntity = _baseRepository.GetSpecificAsync(new[] {entity.Login}).GetAwaiter().GetResult();
        }
    }
}