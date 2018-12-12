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
    public class MusicianDependencyTest
    {
        private readonly IBaseQueryRepository _baseQueryRepository;
        private readonly IMusicianRepository _musicianRepository;
        private readonly TestToolRepository _toolRepository;
        private readonly IBaseCommandRepository _commandRepository;

        public MusicianDependencyTest()
        {
            var db = FunkmapTestDbProvider.DropAndCreateDatabase();

            var storage = new Mock<IFileStorage>().Object;

            var filterServices = new List<IFilterService>() { new MusicianFilterService() };
            IFilterFactory factory = new FilterFactory(filterServices);

            var collection = db.GetCollection<BaseEntity>(CollectionNameProvider.BaseCollectionName);
            var musicianCollection = db.GetCollection<MusicianEntity>(CollectionNameProvider.BaseCollectionName);

            _baseQueryRepository = new BaseQueryRepository(collection, storage, factory);

            _musicianRepository = new MusicianRepository(musicianCollection);

            _toolRepository = new TestToolRepository(collection)
            {
                MusicianCollection = musicianCollection
            };

            var updateBuilders = new List<IUpdateDefinitionBuilder>() { new MusicianUpdateDefinitionBuilder(), new BandUpdateDefinitionBuilder(), new ShopUpdateDefinitionBuilder() };

            var eventBus = new Mock<IEventBus>().Object;
            _commandRepository = new BaseCommandRepository(collection, storage, updateBuilders, eventBus);
        }

        [Fact]
        public void AddDependencyTest()
        {
            var band = new Band
            {
                Login = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString(),
                Location = new Location(12, 12)
            };

            var creationResult = _commandRepository.CreateAsync(new CommandParameter<Profile>() {UserLogin = "rogulenkoko", Parameter = band }).GetAwaiter().GetResult();
            Assert.True(creationResult.Success);

            var musiciansCount = 2;
            var musicians = _toolRepository.GetBandRelatedMusiciansAsync(band.Login, musiciansCount, false).GetAwaiter().GetResult();

            Assert.NotNull(musicians);
            Assert.NotEqual(musicians.Count, 0);
            Assert.True(musicians.Count <= musiciansCount);

            foreach (var musician in musicians)
            {
                Assert.False(musician.BandLogins.Contains(band.Login));
            }

            var fakeUpdatedBand = new Band
            {
                Login = band.Login,
                Name = band.Name,
                Musicians = musicians.Select(x => x.Login).ToList()
            };

            _musicianRepository.ProcessBandDependenciesAsync(band, fakeUpdatedBand).GetAwaiter().GetResult();

            foreach (var musician in musicians)
            {
                var updatedMusician = _baseQueryRepository.GetAsync<Musician>(musician.Login).GetAwaiter().GetResult();
                Assert.True(updatedMusician.BandLogins.Contains(band.Login));
            }
        }

        [Fact]
        public void DeleteDependencyTest()
        {
            var filter = new CommonFilterParameter()
            {
                EntityType = EntityType.Band,
                Take = 1
            };

            var band = _baseQueryRepository.GetFilteredAsync(filter).GetAwaiter().GetResult().SingleOrDefault() as Band;
            Assert.NotNull(band);

            var musiciansCount = 2;
            var musicians = _toolRepository.GetBandRelatedMusiciansAsync(band.Login, musiciansCount, true).GetAwaiter().GetResult();

            //if test data is inconsistent
            band.Musicians = musicians.Select(x => x.Login).ToList();
            Assert.NotNull(musicians);
            Assert.NotEqual(musicians.Count, 0);
            Assert.True(musicians.Count <= musiciansCount);

            var musiciansForDelete = band.Musicians.Take(1).ToList();

            var fakeUpdatedBand = new Band
            {
                Login = band.Login,
                Name = band.Name,
                Musicians = band.Musicians.Except(musiciansForDelete).ToList()
            };

            _musicianRepository.ProcessBandDependenciesAsync(band, fakeUpdatedBand).GetAwaiter().GetResult();

            foreach (var musicianLogin in musiciansForDelete)
            {
                var updatedMusician = _baseQueryRepository.GetAsync<Musician>(musicianLogin).GetAwaiter().GetResult();
                Assert.False(updatedMusician.BandLogins.Contains(band.Login));
            }
        }
    }
}
