using System;
using System.Collections.Generic;
using System.Linq;
using Funkmap.Common.Abstract;
using Funkmap.Common.Cqrs;
using Funkmap.Data;
using Funkmap.Data.Entities.Entities;
using Funkmap.Data.Entities.Entities.Abstract;
using Funkmap.Data.Repositories;
using Funkmap.Data.Services;
using Funkmap.Data.Services.Abstract;
using Funkmap.Data.Services.Update;
using Funkmap.Domain;
using Funkmap.Domain.Abstract.Repositories;
using Funkmap.Domain.Models;
using Funkmap.Domain.Parameters;
using Funkmap.Tests.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Funkmap.Tests
{
    [TestClass]
    public class ProfileDeleteTest
    {

        private IBaseQueryRepository _baseQueryRepository;

        private IBaseCommandRepository _commandRepository;

        [TestInitialize]
        public void Initialize()
        {
            var db = FunkmapTestDbProvider.DropAndCreateDatabase();

            var storage = new Mock<IFileStorage>().Object;

            var filterServices = new List<IFilterService>() { new MusicianFilterService() };
            IFilterFactory factory = new FilterFactory(filterServices);

            var collection = db.GetCollection<BaseEntity>(CollectionNameProvider.BaseCollectionName);
            var bandCollection = db.GetCollection<BandEntity>(CollectionNameProvider.BaseCollectionName);
            var musicianCollection = db.GetCollection<MusicianEntity>(CollectionNameProvider.BaseCollectionName);
            
            _baseQueryRepository = new BaseQueryRepository(collection, storage, factory);

            var eventBus = new InMemoryEventBus();

            var bandRepository = new BandRepository(bandCollection);
            var musicianRepository = new MusicianRepository(musicianCollection);

            var dependenciesController = new BandDependenciesController(bandRepository, musicianRepository, eventBus);
            dependenciesController.InitHandlers();

            var updateBuilders = new List<IUpdateDefenitionBuilder>() { new MusicianUpdateDefenitionBuilder(), new BandUpdateDefenitionBuilder(), new ShopUpdateDefenitionBuilder() };

            _commandRepository = new BaseCommandRepository(collection, storage, updateBuilders, eventBus);
        }

        [TestMethod]
        public void DeleteBandTest()
        {
            //Select any musicians

            var musicianFilter = new CommonFilterParameter()
            {
                EntityType = EntityType.Musician,
                Take = 2,
                Skip = 0
            };

            var musicians = _baseQueryRepository.GetFilteredAsync(musicianFilter).GetAwaiter().GetResult();
            Assert.IsNotNull(musicians);
            Assert.AreEqual(musicians.Count, musicianFilter.Take);


            //Create new band
            var band = new Band()
            {
                Login = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString(),
                Location = new Location(12,12)
            };
            var createResult = _commandRepository.CreateAsync(new CommandParameter<Profile>() {UserLogin = Guid.NewGuid().ToString() , Parameter = band}).GetAwaiter().GetResult();
            Assert.IsTrue(createResult.Success);

            //Add musicians to band
            var bandUpdate = new Band()
            {
                Login = band.Login,
                Musicians = musicians.Select(x => x.Login).ToList()
            };

            var bandUpdateResult = _commandRepository.UpdateAsync(new CommandParameter<Profile>() { UserLogin = band.UserLogin, Parameter = bandUpdate }).GetAwaiter().GetResult();
            Assert.IsTrue(bandUpdateResult.Success);

            var updatedMusicians = _baseQueryRepository.GetFilteredAsync(musicianFilter).GetAwaiter().GetResult().Select(x=>x as Musician).ToList();
            Assert.IsNotNull(updatedMusicians);
            Assert.AreNotEqual(updatedMusicians.Count, 0);
            Assert.IsTrue(updatedMusicians.All(x=>x.BandLogins.Contains(band.Login)));


            var savedBand = _baseQueryRepository.GetAsync<Band>(band.Login).GetAwaiter().GetResult();
            Assert.IsNotNull(savedBand);
            CollectionAssert.AreEqual(savedBand.Musicians, musicians.Select(x=>x.Login).ToList());
            
            var deleteParameter = new CommandParameter<string>()
            {
                UserLogin = savedBand.UserLogin,
                Parameter = savedBand.Login
            };

            //Delete band

            var deleteResult = _commandRepository.DeleteAsync(deleteParameter).GetAwaiter().GetResult();
            Assert.IsTrue(deleteResult.Success);
            Assert.IsNotNull(deleteResult.Body);
            Assert.AreEqual(deleteResult.Body.Login, savedBand.Login);

            var nullBand = _baseQueryRepository.GetAsync(deleteResult.Body.Login).GetAwaiter().GetResult();
            Assert.IsNull(nullBand);


            //Check dependencies clean
            foreach (var musician in musicians)
            {
                Musician savedMusician = _baseQueryRepository.GetAsync<Musician>(musician.Login).GetAwaiter().GetResult();
                Assert.IsTrue(!savedMusician.BandLogins.Contains(deleteResult.Body.Login));
            }
        }

        [TestMethod]
        public void DeleteMusicianTest()
        {
            //Select any bands

            var bandFilter = new CommonFilterParameter()
            {
                EntityType = EntityType.Band,
                Take = 2,
                Skip = 0
            };

            var bands = _baseQueryRepository.GetFilteredAsync(bandFilter).GetAwaiter().GetResult();
            Assert.IsNotNull(bands);
            Assert.AreEqual(bands.Count, bandFilter.Take);


            //CreateAsync new musician
            var musician = new Musician()
            {
                Login = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString(),
                Location = new Location(12, 12)
            };
            var createResult = _commandRepository.CreateAsync(new CommandParameter<Profile>() { UserLogin = Guid.NewGuid().ToString(), Parameter = musician }).GetAwaiter().GetResult();
            Assert.IsTrue(createResult.Success);


            //Add bands to musician
            var musicianUpdate = new Musician()
            {
                Login = musician.Login,
                BandLogins = bands.Select(x => x.Login).ToList()
            };

            var bandUpdateResult = _commandRepository.UpdateAsync(new CommandParameter<Profile>() { UserLogin = musician.UserLogin, Parameter = musicianUpdate }).GetAwaiter().GetResult();
            Assert.IsTrue(bandUpdateResult.Success);
            
            var updatedBands = _baseQueryRepository.GetFilteredAsync(bandFilter).GetAwaiter().GetResult().Select(x => x as Band).ToList();
            Assert.IsNotNull(updatedBands);
            Assert.AreNotEqual(updatedBands.Count, 0);
            Assert.IsTrue(updatedBands.All(x => x.Musicians.Contains(musician.Login)));


            var savedMusician = _baseQueryRepository.GetAsync<Musician>(musician.Login).GetAwaiter().GetResult();
            Assert.IsNotNull(savedMusician);
            CollectionAssert.AreEqual(savedMusician.BandLogins, bands.Select(x => x.Login).ToList());

            var deleteParameter = new CommandParameter<string>()
            {
                UserLogin = savedMusician.UserLogin,
                Parameter = savedMusician.Login
            };

            //Delete musician

            var deleteResult = _commandRepository.DeleteAsync(deleteParameter).GetAwaiter().GetResult();
            Assert.IsTrue(deleteResult.Success);
            Assert.IsNotNull(deleteResult.Body);
            Assert.AreEqual(deleteResult.Body.Login, savedMusician.Login);

            var nullBand = _baseQueryRepository.GetAsync(deleteResult.Body.Login).GetAwaiter().GetResult();
            Assert.IsNull(nullBand);


            //Check dependencies clean
            foreach (var band in bands)
            {
                Band savedBand = _baseQueryRepository.GetAsync<Band>(band.Login).GetAwaiter().GetResult();
                Assert.IsTrue(!savedBand.Musicians.Contains(deleteResult.Body.Login));
            }
        }
    }
}
