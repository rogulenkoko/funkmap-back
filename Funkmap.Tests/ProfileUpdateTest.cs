using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Funkmap.Common.Abstract;
using Funkmap.Cqrs;
using Funkmap.Data;
using Funkmap.Data.Entities.Entities;
using Funkmap.Data.Entities.Entities.Abstract;
using Funkmap.Data.Repositories;
using Funkmap.Data.Services;
using Funkmap.Data.Services.Abstract;
using Funkmap.Data.Services.Update;
using Funkmap.Domain;
using Funkmap.Domain.Abstract.Repositories;
using Funkmap.Domain.Enums;
using Funkmap.Domain.Models;
using Funkmap.Domain.Parameters;
using Funkmap.Tests.Data;
using Funkmap.Tests.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Funkmap.Tests
{
    [TestClass]
    public class ProfileUpdateTest
    {
        private IBaseQueryRepository _baseQueryRepository;

        private IBaseCommandRepository _commandRepository;

        private TestToolRepository _toolRepository;

        [TestInitialize]
        public void Initialize()
        {
            var db = FunkmapTestDbProvider.DropAndCreateDatabase();

            var storage = new Mock<IFileStorage>().Object;

            var filterServices = new List<IFilterService>() { new MusicianFilterService() };
            IFilterFactory factory = new FilterFactory(filterServices);

            var collection = db.GetCollection<BaseEntity>(CollectionNameProvider.BaseCollectionName);

            var updateBuilders = new List<IUpdateDefenitionBuilder>() { new MusicianUpdateDefenitionBuilder(), new BandUpdateDefenitionBuilder(), new ShopUpdateDefenitionBuilder() };

            _baseQueryRepository = new BaseQueryRepository(collection, storage, factory);

            var bandCollection = db.GetCollection<BandEntity>(CollectionNameProvider.BaseCollectionName);
            var musicianCollection = db.GetCollection<MusicianEntity>(CollectionNameProvider.BaseCollectionName);

            var eventBus = new InMemoryEventBus();

            var bandRepository = new BandRepository(bandCollection);
            var musicianRepository = new MusicianRepository(musicianCollection);

            var dependenciesController = new BandDependenciesController(bandRepository, musicianRepository, eventBus);
            dependenciesController.InitHandlers();

            _commandRepository = new BaseCommandRepository(collection, storage, updateBuilders, eventBus);

            _toolRepository = new TestToolRepository(collection);
        }

        [TestMethod]
        public void CreateProfileTest()
        {
            var musician = GetMusician();

            var nullMusician = _baseQueryRepository.GetAsync(musician.Login).GetAwaiter().GetResult();
            Assert.IsNull(nullMusician);

            var parameter = new CommandParameter<Profile>()
            {
                UserLogin = musician.UserLogin,
                Parameter = musician
            };

            _commandRepository.CreateAsync(parameter).GetAwaiter().GetResult();

            Domain.Models.Musician savedMusician = _baseQueryRepository.GetAsync(musician.Login).GetAwaiter().GetResult() as Domain.Models.Musician;
            Assert.IsNotNull(savedMusician);

            Assert.AreEqual(musician.Expirience, savedMusician.Expirience);
            Assert.AreEqual(musician.Instrument, savedMusician.Instrument);
            Assert.AreEqual(musician.Name, savedMusician.Name);
            Assert.AreEqual(musician.Address, savedMusician.Address);

            Assert.AreEqual(musician.BirthDate.ToString(), savedMusician.BirthDate.ToString());
            Assert.AreEqual(musician.Description, savedMusician.Description);
            Assert.AreEqual(musician.Login, savedMusician.Login);
            Assert.AreEqual(musician.Sex, savedMusician.Sex);
            Assert.AreEqual(musician.FacebookLink, savedMusician.FacebookLink);
            Assert.AreEqual(musician.VkLink, savedMusician.VkLink);
            Assert.AreEqual(musician.YoutubeLink, savedMusician.YoutubeLink);
            Assert.AreEqual(musician.SoundCloudLink, savedMusician.SoundCloudLink);

            CollectionAssert.AreEqual(musician.Styles, savedMusician.Styles);
        }

        [TestMethod]
        public void DeleteProfileTest()
        {
            var login = _toolRepository.GetAnyLoginsAsync(1).GetAwaiter().GetResult().First();
            var savedMusician = _baseQueryRepository.GetAsync(login).GetAwaiter().GetResult();
            Assert.IsNotNull(savedMusician);
            Assert.AreEqual(savedMusician.Login, login);

            var parameter = new CommandParameter<string>()
            {
                UserLogin = savedMusician.UserLogin,
                Parameter = login
            };

            var deletedResult = _commandRepository.DeleteAsync(parameter).GetAwaiter().GetResult();
            Assert.IsTrue(deletedResult.Success);
            Assert.AreEqual(deletedResult.Body.Login, savedMusician.Login);

            var deleted = _baseQueryRepository.GetAsync(login).GetAwaiter().GetResult();
            Assert.IsNull(deleted);
        }

        [TestMethod]
        public void UpdateBaseTest()
        {
            var login = _toolRepository.GetAnyLoginsAsync(1).GetAwaiter().GetResult().SingleOrDefault();
            Assert.IsNotNull(login);

            var existingProfile = _baseQueryRepository.GetAsync(login).GetAwaiter().GetResult();
            Assert.IsNotNull(existingProfile);

            //positive

            var profileUpdate = Activator.CreateInstance(existingProfile.GetType()) as Profile;
            Assert.IsNotNull(profileUpdate);

            profileUpdate.Login = existingProfile.Login;

            profileUpdate.Name = $"{existingProfile.Name}_updated";
            profileUpdate.Address = $"{existingProfile.Address}_updated";
            profileUpdate.Description = $"{existingProfile.Description}_updated";
            profileUpdate.FacebookLink = $"{existingProfile.FacebookLink}_updated";
            profileUpdate.VkLink = $"{existingProfile.VkLink}_updated";
            profileUpdate.YoutubeLink = $"{existingProfile.YoutubeLink}_updated";
            profileUpdate.SoundCloudLink = $"{existingProfile.SoundCloudLink}_updated";
            profileUpdate.Location = new Location(existingProfile.Location.Latitude + 1, existingProfile.Location.Longitude + 1);
            profileUpdate.IsActive = existingProfile.IsActive.HasValue ? !existingProfile.IsActive : false;
            profileUpdate.VideoInfos = new List<VideoInfo>()
            {
                new VideoInfo() {Type = VideoType.Youtube, Id = Guid.NewGuid().ToString()},
                new VideoInfo() {Type = VideoType.Youtube, Id = Guid.NewGuid().ToString()}
            };

            var random = new Random();
            profileUpdate.SoundCloudTracks = new List<AudioInfo>()
            {
                new AudioInfo() {Id = random.Next(0, 1000)},
                new AudioInfo() {Id = random.Next(0, 1000)}
            };

            var parameter = new CommandParameter<Profile>()
            {
                UserLogin = existingProfile.UserLogin,
                Parameter = profileUpdate
            };

            var result = _commandRepository.UpdateAsync(parameter).GetAwaiter().GetResult().Success;
            Assert.IsTrue(result);

            var updatedProfile = _baseQueryRepository.GetAsync(login).GetAwaiter().GetResult();
            Assert.AreEqual(updatedProfile.Name, profileUpdate.Name);
            Assert.AreNotEqual(existingProfile.Name, updatedProfile.Name);

            Assert.AreEqual(updatedProfile.Address, profileUpdate.Address);
            Assert.AreNotEqual(existingProfile.Address, updatedProfile.Address);

            Assert.AreEqual(updatedProfile.Description, profileUpdate.Description);
            Assert.AreNotEqual(existingProfile.Description, updatedProfile.Description);

            Assert.AreEqual(updatedProfile.FacebookLink, profileUpdate.FacebookLink);
            Assert.AreNotEqual(existingProfile.FacebookLink, updatedProfile.FacebookLink);

            Assert.AreEqual(updatedProfile.VkLink, profileUpdate.VkLink);
            Assert.AreNotEqual(existingProfile.VkLink, updatedProfile.VkLink);

            Assert.AreEqual(updatedProfile.SoundCloudLink, profileUpdate.SoundCloudLink);
            Assert.AreNotEqual(existingProfile.SoundCloudLink, updatedProfile.SoundCloudLink);

            Assert.AreEqual(updatedProfile.YoutubeLink, profileUpdate.YoutubeLink);
            Assert.AreNotEqual(existingProfile.YoutubeLink, updatedProfile.YoutubeLink);

            Assert.IsTrue(updatedProfile.Location.Equals(profileUpdate.Location));
            Assert.IsFalse(existingProfile.Location.Equals(updatedProfile.Location));

            Assert.AreEqual(updatedProfile.IsActive, profileUpdate.IsActive);
            Assert.AreNotEqual(existingProfile.IsActive, updatedProfile.IsActive);

            CollectionAssert.AreEqual(updatedProfile.SoundCloudTracks, profileUpdate.SoundCloudTracks);
            CollectionAssert.AreNotEqual(existingProfile.SoundCloudTracks, updatedProfile.SoundCloudTracks);

            CollectionAssert.AreEqual(updatedProfile.VideoInfos, profileUpdate.VideoInfos);
            CollectionAssert.AreNotEqual(existingProfile.VideoInfos, updatedProfile.VideoInfos);


            //negative

            profileUpdate.EntityType = Enum.GetValues(typeof(EntityType)).Cast<EntityType>().First(x => x != existingProfile.EntityType && x != EntityType.None);
            profileUpdate.UserLogin = $"{updatedProfile.UserLogin}_updated";
            profileUpdate.AvatarUrl = $"{updatedProfile.AvatarUrl}_updated";
            profileUpdate.AvatarMiniUrl = $"{updatedProfile.AvatarMiniUrl}_updated";

            parameter = new CommandParameter<Profile>
            {
                UserLogin = existingProfile.UserLogin,
                Parameter = profileUpdate
            };

            result = _commandRepository.UpdateAsync(parameter).GetAwaiter().GetResult().Success;
            Assert.IsTrue(result);

            updatedProfile = _baseQueryRepository.GetAsync(login).GetAwaiter().GetResult();

            Assert.AreNotEqual(profileUpdate.UserLogin, updatedProfile.UserLogin);
            Assert.AreEqual(existingProfile.UserLogin, updatedProfile.UserLogin);

            Assert.AreNotEqual(profileUpdate.AvatarUrl, updatedProfile.AvatarUrl);
            Assert.AreEqual(existingProfile.AvatarUrl, updatedProfile.AvatarUrl);

            Assert.AreNotEqual(profileUpdate.AvatarMiniUrl, updatedProfile.AvatarMiniUrl);
            Assert.AreEqual(existingProfile.AvatarMiniUrl, updatedProfile.AvatarMiniUrl);

            profileUpdate.Login = Guid.NewGuid().ToString();
            profileUpdate.Name = Guid.NewGuid().ToString();

            parameter = new CommandParameter<Profile>()
            {
                UserLogin = existingProfile.UserLogin,
                Parameter = profileUpdate
            };

            result = _commandRepository.UpdateAsync(parameter).GetAwaiter().GetResult().Success;
            Assert.IsFalse(result);

        }

        [TestMethod]
        public void UpdateMusicianTest()
        {
            var filtered = _baseQueryRepository.GetFilteredAsync(new CommonFilterParameter
            {
                EntityType = EntityType.Musician,
                Take = 1,
                Skip = 0
            }).GetAwaiter().GetResult();

            Assert.AreNotEqual(filtered.Count, 0);


            var profile = filtered.Single() as Domain.Models.Musician;
            Assert.IsNotNull(profile);

            Domain.Models.Musician existingEntity = _baseQueryRepository.GetAsync(profile.Login).GetAwaiter().GetResult() as Domain.Models.Musician;
            Assert.IsNotNull(existingEntity);


            var profileUpdates = new Domain.Models.Musician()
            {
                Login = existingEntity.Login
            };

            profileUpdates.Instrument = Enum.GetValues(typeof(Instruments)).Cast<Instruments>().First(x => x != existingEntity.Instrument && x != Instruments.None);
            profileUpdates.BirthDate = profileUpdates.BirthDate?.AddYears(-1) ?? DateTime.Now.AddYears(-20);
            profileUpdates.Expirience = Enum.GetValues(typeof(Expiriences)).Cast<Expiriences>().First(x => x != existingEntity.Expirience && x != Expiriences.None);
            profileUpdates.Sex = Enum.GetValues(typeof(Sex)).Cast<Sex>().First(x => x != existingEntity.Sex && x != Sex.None);
            profileUpdates.Styles = Enum.GetValues(typeof(Styles)).Cast<Styles>().Except(existingEntity.Styles).ToList();

            var parameter = new CommandParameter<Profile>()
            {
                UserLogin = existingEntity.UserLogin,
                Parameter = profileUpdates
            };

            var result = _commandRepository.UpdateAsync(parameter).GetAwaiter().GetResult().Success;
            Assert.IsTrue(result);

            var updatedProfile = _baseQueryRepository.GetAsync(profile.Login).GetAwaiter().GetResult() as Domain.Models.Musician;
            Assert.IsNotNull(updatedProfile);

            Assert.AreEqual(profileUpdates.Instrument, updatedProfile.Instrument);
            Assert.AreNotEqual(existingEntity.Instrument, updatedProfile.Instrument);

            Assert.AreEqual(profileUpdates.BirthDate.ToString(), updatedProfile.BirthDate.ToString());
            Assert.AreNotEqual(existingEntity.BirthDate.ToString(), updatedProfile.BirthDate.ToString());

            Assert.AreEqual(profileUpdates.Expirience, updatedProfile.Expirience);
            Assert.AreNotEqual(existingEntity.Expirience, updatedProfile.Expirience);

            Assert.AreEqual(profileUpdates.Sex, updatedProfile.Sex);
            Assert.AreNotEqual(existingEntity.Sex, updatedProfile.Sex);

            CollectionAssert.AreEqual(profileUpdates.Styles, updatedProfile.Styles);
            CollectionAssert.AreNotEqual(existingEntity.Styles, updatedProfile.Styles);
        }

        [TestMethod]
        public void UpdateBandTest()
        {
            var filtered = _baseQueryRepository.GetFilteredAsync(new CommonFilterParameter
            {
                EntityType = EntityType.Band,
                Take = 1,
                Skip = 0
            }).GetAwaiter().GetResult();

            Assert.AreNotEqual(filtered.Count, 0);


            var profile = filtered.Single() as Band;
            Assert.IsNotNull(profile);

            Band existingEntity = _baseQueryRepository.GetAsync(profile.Login).GetAwaiter().GetResult() as Band;
            Assert.IsNotNull(existingEntity);


            var profileUpdates = new Band
            {
                Login = existingEntity.Login
            };

            profileUpdates.Styles = Enum.GetValues(typeof(Styles)).Cast<Styles>().Except(existingEntity.Styles).ToList();
            profileUpdates.DesiredInstruments = Enum.GetValues(typeof(Instruments)).Cast<Instruments>().Except(existingEntity.DesiredInstruments).ToList();
            profileUpdates.InvitedMusicians = new List<string>() { Guid.NewGuid().ToString() };
            profileUpdates.Musicians = new List<string>() { Guid.NewGuid().ToString() };

            var parameter = new CommandParameter<Profile>()
            {
                UserLogin = existingEntity.UserLogin,
                Parameter = profileUpdates
            };

            var result = _commandRepository.UpdateAsync(parameter).GetAwaiter().GetResult().Success;
            Assert.IsTrue(result);

            var updatedProfile = _baseQueryRepository.GetAsync(profile.Login).GetAwaiter().GetResult() as Band;
            Assert.IsNotNull(updatedProfile);

            CollectionAssert.AreEqual(profileUpdates.Styles, updatedProfile.Styles);
            CollectionAssert.AreNotEqual(existingEntity.Styles, updatedProfile.Styles);

            CollectionAssert.AreEqual(profileUpdates.DesiredInstruments, updatedProfile.DesiredInstruments);
            CollectionAssert.AreNotEqual(existingEntity.DesiredInstruments, updatedProfile.DesiredInstruments);

            CollectionAssert.AreEqual(profileUpdates.InvitedMusicians, updatedProfile.InvitedMusicians);
            CollectionAssert.AreNotEqual(existingEntity.InvitedMusicians, updatedProfile.InvitedMusicians);

            CollectionAssert.AreEqual(profileUpdates.Musicians, updatedProfile.Musicians);
            CollectionAssert.AreNotEqual(existingEntity.Musicians, updatedProfile.Musicians);
        }

        [TestMethod]
        public void UpdateShopTest()
        {
            var filtered = _baseQueryRepository.GetFilteredAsync(new CommonFilterParameter
            {
                EntityType = EntityType.Shop,
                Take = 1,
                Skip = 0
            }).GetAwaiter().GetResult();

            Assert.AreNotEqual(filtered.Count, 0);


            var profile = filtered.Single() as Shop;
            Assert.IsNotNull(profile);

            Shop existingEntity = _baseQueryRepository.GetAsync(profile.Login).GetAwaiter().GetResult() as Shop;
            Assert.IsNotNull(existingEntity);


            var profileUpdates = new Shop
            {
                Login = existingEntity.Login
            };

            profileUpdates.Website = $"{existingEntity.Website}_updated";

            var parameter = new CommandParameter<Profile>()
            {
                UserLogin = existingEntity.UserLogin,
                Parameter = profileUpdates
            };

            var result = _commandRepository.UpdateAsync(parameter).GetAwaiter().GetResult().Success;
            Assert.IsTrue(result);

            var updatedProfile = _baseQueryRepository.GetAsync(profile.Login).GetAwaiter().GetResult() as Shop;
            Assert.IsNotNull(updatedProfile);

            Assert.AreEqual(profileUpdates.Website, updatedProfile.Website);
            Assert.AreNotEqual(existingEntity.Website, updatedProfile.Website);
        }

        [TestMethod]
        public void UpdateMusicianWithDependencies()
        {
            var band = new Band
            {
                Login = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString(),
                Location = new Location(13, 13)
            };

            var createResult = _commandRepository.CreateAsync(new CommandParameter<Profile>() { UserLogin = "rogulenkoko", Parameter = band }).GetAwaiter().GetResult();
            Assert.IsTrue(createResult.Success);

            band.Musicians = new List<string>();

            var musicianFilter = new CommonFilterParameter()
            {
                EntityType = EntityType.Musician,
                Take = 2,
                Skip = 0
            };

            var musicians = _baseQueryRepository.GetFilteredAsync(musicianFilter).GetAwaiter().GetResult();
            Assert.IsNotNull(musicians);
            Assert.AreEqual(musicians.Count, musicianFilter.Take);

            var bandUpdate = new Band
            {
                Login = band.Login,
                EntityType = band.EntityType,
                Musicians = musicians.Select(x => x.Login).ToList()
            };

            var paramter = new CommandParameter<Profile>()
            {
                UserLogin = band.UserLogin,
                Parameter = bandUpdate
            };

            var updateResult = _commandRepository.UpdateAsync(paramter).GetAwaiter().GetResult();
            Assert.IsTrue(updateResult.Success);

            //waiting for event bus execution
            Thread.Sleep(3000);

            musicians.ForEach(musician =>
            {
                var updatedMusician = _baseQueryRepository.GetAsync<Musician>(musician.Login).GetAwaiter().GetResult();
                Assert.IsTrue(updatedMusician.BandLogins.Contains(band.Login));
            });
        }

        private Domain.Models.Musician GetMusician()
        {
            return new Domain.Models.Musician
            {
                Name = "qwerty",
                Address = "Spb",
                BirthDate = DateTime.Now.AddYears(-20),
                Description = "qweqweqwe",
                Login = Guid.NewGuid().ToString(),
                Expirience = Expiriences.Begginer,
                Instrument = Instruments.Bass,
                Sex = Sex.Female,
                FacebookLink = "facebook",
                VkLink = "Vk",
                Location = new Location(12, 40),
                UserLogin = "rogulenkoko",
                YoutubeLink = "YT",
                Styles = new List<Styles>() { Styles.Electronic, Styles.HipHop, Styles.Jazz }

            };
        }
    }
}
