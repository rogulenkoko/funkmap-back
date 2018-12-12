using System;
using System.Collections.Generic;
using System.Linq;
using Funkmap.Common.Abstract;
using Funkmap.Cqrs.Abstract;
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
using Funkmap.Tests.Tools;
using Moq;
using Xunit;

namespace Funkmap.Tests
{
    public class BandDependencyTest
    {
        private readonly IBaseQueryRepository _baseQueryRepository;
        private readonly IBandRepository _bandRepository;
        private readonly TestToolRepository _toolRepository;
        private readonly IBaseCommandRepository _commandRepository;

        public BandDependencyTest()
        {
            var db = FunkmapTestDbProvider.DropAndCreateDatabase();

            var storage = new Mock<IFileStorage>().Object;

            var filterServices = new List<IFilterService>() { new MusicianFilterService() };
            IFilterFactory factory = new FilterFactory(filterServices);

            var collection = db.GetCollection<BaseEntity>(CollectionNameProvider.BaseCollectionName);
            var bandCollection = db.GetCollection<BandEntity>(CollectionNameProvider.BaseCollectionName);

            _baseQueryRepository = new BaseQueryRepository(collection, storage, factory);

            _bandRepository = new BandRepository(bandCollection);

            _toolRepository = new TestToolRepository(collection)
            {
                BandCollection = bandCollection
            };

            var updateBuilders = new List<IUpdateDefinitionBuilder>() { new MusicianUpdateDefinitionBuilder(), new BandUpdateDefinitionBuilder(), new ShopUpdateDefinitionBuilder() };

            var eventBus = new Mock<IEventBus>().Object;
            _commandRepository = new BaseCommandRepository(collection, storage, updateBuilders, eventBus);
        }

        [Fact]
        public void AddDependencyTest()
        {
            var musician = new Musician
            {
                Login = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString(),
                Location = new Location(12, 12)
            };

            var creationResult = _commandRepository.CreateAsync(new CommandParameter<Profile>() { UserLogin = "rogulenkoko", Parameter = musician }).GetAwaiter().GetResult();
            Assert.True(creationResult.Success);

            var bandsCount = 2;
            var bands = _toolRepository.GetMusicianRelatedBandsAsync(musician.Login, bandsCount, false).GetAwaiter().GetResult();

            Assert.NotNull(bands);
            Assert.NotEqual(bands.Count, 0);
            Assert.True(bands.Count <= bandsCount);

            foreach (var band in bands)
            {
                Assert.False(band.Musicians.Contains(band.Login));
            }

            var fakeUpdatedMusician = new Musician
            {
                Login = musician.Login,
                Name = musician.Name,
                BandLogins = bands.Select(x => x.Login).ToList()
            };

            _bandRepository.ProcessMusicianDependenciesAsync(musician, fakeUpdatedMusician).GetAwaiter().GetResult();

            foreach (var band in bands)
            {
                var updatedBand = _baseQueryRepository.GetAsync<Band>(band.Login).GetAwaiter().GetResult();
                Assert.True(updatedBand.Musicians.Contains(musician.Login));
            }
        }

        [Fact]
        public void DeleteDependencyTest()
        {
            var filter = new CommonFilterParameter
            {
                EntityType = EntityType.Musician,
                Take = 1
            };

            var musician = _baseQueryRepository.GetFilteredAsync(filter).GetAwaiter().GetResult().SingleOrDefault() as Musician;
            Assert.NotNull(musician);

            var bandsCount = 2;
            var bands = _toolRepository.GetMusicianRelatedBandsAsync(musician.Login, bandsCount, true).GetAwaiter().GetResult();

            //if test data is inconsistent
            musician.BandLogins = bands.Select(x => x.Login).ToList();
            Assert.NotNull(bands);
            Assert.NotEqual(bands.Count, 0);
            Assert.True(bands.Count <= bandsCount);

            var bandsForDelete = musician.BandLogins.Take(1).ToList();

            var fakeUpdatedMusician = new Musician()
            {
                Login = musician.Login,
                Name = musician.Name,
                BandLogins = musician.BandLogins.Except(bandsForDelete).ToList()
            };

            _bandRepository.ProcessMusicianDependenciesAsync(musician, fakeUpdatedMusician).GetAwaiter().GetResult();

            foreach (var bandLogin in bandsForDelete)
            {
                var updatedBand = _baseQueryRepository.GetAsync<Band>(bandLogin).GetAwaiter().GetResult();
                Assert.False(updatedBand.Musicians.Contains(musician.Login));
            }
        }
    }
}
