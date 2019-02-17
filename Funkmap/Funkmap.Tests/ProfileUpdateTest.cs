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
using Moq;
using Xunit;

namespace Funkmap.Tests
{
    public class ProfileUpdateTest
    {
        private readonly IBaseQueryRepository _baseQueryRepository;
        private readonly IBaseCommandRepository _commandRepository;
        private readonly TestToolRepository _toolRepository;

        public ProfileUpdateTest()
        {
            var db = FunkmapTestDbProvider.DropAndCreateDatabase();

            var storage = new Mock<IFileStorage>().Object;

            var filterServices = new List<IFilterService>() { new MusicianFilterService() };
            IFilterFactory factory = new FilterFactory(filterServices);

            var collection = db.GetCollection<BaseEntity>(CollectionNameProvider.BaseCollectionName);

            var updateBuilders = new List<IUpdateDefinitionBuilder>() { new MusicianUpdateDefinitionBuilder(), new BandUpdateDefinitionBuilder(), new ShopUpdateDefinitionBuilder() };

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

        [Fact]
        public void CreateProfileTest()
        {
            var musician = GetMusician();

            var nullMusician = _baseQueryRepository.GetAsync(musician.Login).GetAwaiter().GetResult();
            Assert.Null(nullMusician);

            var parameter = new CommandParameter<Profile>()
            {
                UserLogin = musician.UserLogin,
                Parameter = musician
            };

            _commandRepository.CreateAsync(parameter).GetAwaiter().GetResult();

            var savedMusician = _baseQueryRepository.GetAsync(musician.Login).GetAwaiter().GetResult() as Musician;
            Assert.NotNull(savedMusician);

            Assert.Equal(musician.Experience, savedMusician.Experience);
            Assert.Equal(musician.Instrument, savedMusician.Instrument);
            Assert.Equal(musician.Name, savedMusician.Name);
            Assert.Equal(musician.Address, savedMusician.Address);

            Assert.Equal(musician.BirthDate.ToString(), savedMusician.BirthDate.ToString());
            Assert.Equal(musician.Description, savedMusician.Description);
            Assert.Equal(musician.Login, savedMusician.Login);
            Assert.Equal(musician.Sex, savedMusician.Sex);
            Assert.Equal(musician.FacebookLink, savedMusician.FacebookLink);
            Assert.Equal(musician.VkLink, savedMusician.VkLink);
            Assert.Equal(musician.YoutubeLink, savedMusician.YoutubeLink);
            Assert.Equal(musician.SoundCloudLink, savedMusician.SoundCloudLink);

            Assert.Equal(musician.Styles, savedMusician.Styles);
        }

        [Fact]
        public void DeleteProfileTest()
        {
            var login = _toolRepository.GetAnyLoginsAsync(1).GetAwaiter().GetResult().First();
            var savedMusician = _baseQueryRepository.GetAsync(login).GetAwaiter().GetResult();
            Assert.NotNull(savedMusician);
            Assert.Equal(savedMusician.Login, login);

            var parameter = new CommandParameter<string>()
            {
                UserLogin = savedMusician.UserLogin,
                Parameter = login
            };

            var deletedResult = _commandRepository.DeleteAsync(parameter).GetAwaiter().GetResult();
            Assert.True(deletedResult.Success);
            Assert.Equal(deletedResult.Body.Login, savedMusician.Login);

            var deleted = _baseQueryRepository.GetAsync(login).GetAwaiter().GetResult();
            Assert.Null(deleted);
        }

        [Fact]
        public void UpdateBaseTest()
        {
            var login = _toolRepository.GetAnyLoginsAsync(1).GetAwaiter().GetResult().SingleOrDefault();
            Assert.NotNull(login);

            var existingProfile = _baseQueryRepository.GetAsync(login).GetAwaiter().GetResult();
            Assert.NotNull(existingProfile);

            //positive

            var profileUpdate = Activator.CreateInstance(existingProfile.GetType()) as Profile;
            Assert.NotNull(profileUpdate);

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
            Assert.True(result);

            var updatedProfile = _baseQueryRepository.GetAsync(login).GetAwaiter().GetResult();
            Assert.Equal(updatedProfile.Name, profileUpdate.Name);
            Assert.NotEqual(existingProfile.Name, updatedProfile.Name);

            Assert.Equal(updatedProfile.Address, profileUpdate.Address);
            Assert.NotEqual(existingProfile.Address, updatedProfile.Address);

            Assert.Equal(updatedProfile.Description, profileUpdate.Description);
            Assert.NotEqual(existingProfile.Description, updatedProfile.Description);

            Assert.Equal(updatedProfile.FacebookLink, profileUpdate.FacebookLink);
            Assert.NotEqual(existingProfile.FacebookLink, updatedProfile.FacebookLink);

            Assert.Equal(updatedProfile.VkLink, profileUpdate.VkLink);
            Assert.NotEqual(existingProfile.VkLink, updatedProfile.VkLink);

            Assert.Equal(updatedProfile.SoundCloudLink, profileUpdate.SoundCloudLink);
            Assert.NotEqual(existingProfile.SoundCloudLink, updatedProfile.SoundCloudLink);

            Assert.Equal(updatedProfile.YoutubeLink, profileUpdate.YoutubeLink);
            Assert.NotEqual(existingProfile.YoutubeLink, updatedProfile.YoutubeLink);

            Assert.True(updatedProfile.Location.Equals(profileUpdate.Location));
            Assert.False(existingProfile.Location.Equals(updatedProfile.Location));

            Assert.Equal(updatedProfile.IsActive, profileUpdate.IsActive);
            Assert.NotEqual(existingProfile.IsActive, updatedProfile.IsActive);

            Assert.Equal(updatedProfile.SoundCloudTracks, profileUpdate.SoundCloudTracks);
            Assert.NotEqual(existingProfile.SoundCloudTracks, updatedProfile.SoundCloudTracks);

            Assert.Equal(updatedProfile.VideoInfos, profileUpdate.VideoInfos);
            Assert.NotEqual(existingProfile.VideoInfos, updatedProfile.VideoInfos);


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
            Assert.True(result);

            updatedProfile = _baseQueryRepository.GetAsync(login).GetAwaiter().GetResult();

            Assert.NotEqual(profileUpdate.UserLogin, updatedProfile.UserLogin);
            Assert.Equal(existingProfile.UserLogin, updatedProfile.UserLogin);

            Assert.NotEqual(profileUpdate.AvatarUrl, updatedProfile.AvatarUrl);
            Assert.Equal(existingProfile.AvatarUrl, updatedProfile.AvatarUrl);

            Assert.NotEqual(profileUpdate.AvatarMiniUrl, updatedProfile.AvatarMiniUrl);
            Assert.Equal(existingProfile.AvatarMiniUrl, updatedProfile.AvatarMiniUrl);

            profileUpdate.Login = Guid.NewGuid().ToString();
            profileUpdate.Name = Guid.NewGuid().ToString();

            parameter = new CommandParameter<Profile>()
            {
                UserLogin = existingProfile.UserLogin,
                Parameter = profileUpdate
            };

            result = _commandRepository.UpdateAsync(parameter).GetAwaiter().GetResult().Success;
            Assert.False(result);

        }

        [Fact]
        public void UpdateMusicianTest()
        {
            var filtered = _baseQueryRepository.GetFilteredAsync(new CommonFilterParameter
            {
                EntityType = EntityType.Musician,
                Take = 1,
                Skip = 0
            }).GetAwaiter().GetResult();

            Assert.NotEmpty(filtered);


            var profile = filtered.Single() as Musician;
            Assert.NotNull(profile);

            var existingEntity = _baseQueryRepository.GetAsync(profile.Login).GetAwaiter().GetResult() as Musician;
            Assert.NotNull(existingEntity);


            var profileUpdates = new Musician
            {
                Login = existingEntity.Login,
                Instrument = Enum.GetValues(typeof(Instruments)).Cast<Instruments>()
                    .First(x => x != existingEntity.Instrument && x != Instruments.None)
            };

            profileUpdates.BirthDate = profileUpdates.BirthDate?.AddYears(-1) ?? DateTime.Now.AddYears(-20);
            profileUpdates.Experience = Enum.GetValues(typeof(Experiences)).Cast<Experiences>().First(x => x != existingEntity.Experience && x != Experiences.None);
            profileUpdates.Sex = Enum.GetValues(typeof(Sex)).Cast<Sex>().First(x => x != existingEntity.Sex && x != Sex.None);
            profileUpdates.Styles = Enum.GetValues(typeof(Styles)).Cast<Styles>().Except(existingEntity.Styles).ToList();

            var parameter = new CommandParameter<Profile>()
            {
                UserLogin = existingEntity.UserLogin,
                Parameter = profileUpdates
            };

            var result = _commandRepository.UpdateAsync(parameter).GetAwaiter().GetResult().Success;
            Assert.True(result);

            var updatedProfile = _baseQueryRepository.GetAsync(profile.Login).GetAwaiter().GetResult() as Musician;
            Assert.NotNull(updatedProfile);

            Assert.Equal(profileUpdates.Instrument, updatedProfile.Instrument);
            Assert.NotEqual(existingEntity.Instrument, updatedProfile.Instrument);

            Assert.Equal(profileUpdates.BirthDate.ToString(), updatedProfile.BirthDate.ToString());
            Assert.NotEqual(existingEntity.BirthDate.ToString(), updatedProfile.BirthDate.ToString());

            Assert.Equal(profileUpdates.Experience, updatedProfile.Experience);
            Assert.NotEqual(existingEntity.Experience, updatedProfile.Experience);

            Assert.Equal(profileUpdates.Sex, updatedProfile.Sex);
            Assert.NotEqual(existingEntity.Sex, updatedProfile.Sex);

            Assert.Equal(profileUpdates.Styles, updatedProfile.Styles);
            Assert.NotEqual(existingEntity.Styles, updatedProfile.Styles);
        }

        [Fact]
        public void UpdateBandTest()
        {
            var filtered = _baseQueryRepository.GetFilteredAsync(new CommonFilterParameter
            {
                EntityType = EntityType.Band,
                Take = 1,
                Skip = 0
            }).GetAwaiter().GetResult();

            Assert.NotEmpty(filtered);


            var profile = filtered.Single() as Band;
            Assert.NotNull(profile);

            Band existingEntity = _baseQueryRepository.GetAsync(profile.Login).GetAwaiter().GetResult() as Band;
            Assert.NotNull(existingEntity);


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
            Assert.True(result);

            var updatedProfile = _baseQueryRepository.GetAsync(profile.Login).GetAwaiter().GetResult() as Band;
            Assert.NotNull(updatedProfile);

            Assert.Equal(profileUpdates.Styles, updatedProfile.Styles);
            Assert.NotEqual(existingEntity.Styles, updatedProfile.Styles);

            Assert.Equal(profileUpdates.DesiredInstruments, updatedProfile.DesiredInstruments);
            Assert.NotEqual(existingEntity.DesiredInstruments, updatedProfile.DesiredInstruments);

            Assert.Equal(profileUpdates.InvitedMusicians, updatedProfile.InvitedMusicians);
            Assert.NotEqual(existingEntity.InvitedMusicians, updatedProfile.InvitedMusicians);

            Assert.Equal(profileUpdates.Musicians, updatedProfile.Musicians);
            Assert.NotEqual(existingEntity.Musicians, updatedProfile.Musicians);
        }

        [Fact]
        public void UpdateShopTest()
        {
            var filtered = _baseQueryRepository.GetFilteredAsync(new CommonFilterParameter
            {
                EntityType = EntityType.Shop,
                Take = 1,
                Skip = 0
            }).GetAwaiter().GetResult();

            Assert.NotEmpty(filtered);


            var profile = filtered.Single() as Shop;
            Assert.NotNull(profile);

            Shop existingEntity = _baseQueryRepository.GetAsync(profile.Login).GetAwaiter().GetResult() as Shop;
            Assert.NotNull(existingEntity);


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
            Assert.True(result);

            var updatedProfile = _baseQueryRepository.GetAsync(profile.Login).GetAwaiter().GetResult() as Shop;
            Assert.NotNull(updatedProfile);

            Assert.Equal(profileUpdates.Website, updatedProfile.Website);
            Assert.NotEqual(existingEntity.Website, updatedProfile.Website);
        }

        [Fact]
        public void UpdateMusicianWithDependencies()
        {
            var band = new Band
            {
                Login = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString(),
                Location = new Location(13, 13)
            };

            var createResult = _commandRepository.CreateAsync(new CommandParameter<Profile>() { UserLogin = "rogulenkoko", Parameter = band }).GetAwaiter().GetResult();
            Assert.True(createResult.Success);

            band.Musicians = new List<string>();

            var musicianFilter = new CommonFilterParameter()
            {
                EntityType = EntityType.Musician,
                Take = 2,
                Skip = 0
            };

            var musicians = _baseQueryRepository.GetFilteredAsync(musicianFilter).GetAwaiter().GetResult();
            Assert.NotNull(musicians);
            Assert.Equal(musicians.Count, musicianFilter.Take);

            var bandUpdate = new Band
            {
                Login = band.Login,
                EntityType = band.EntityType,
                Musicians = musicians.Select(x => x.Login).ToList()
            };

            var parameter = new CommandParameter<Profile>
            {
                UserLogin = band.UserLogin,
                Parameter = bandUpdate
            };

            var updateResult = _commandRepository.UpdateAsync(parameter ).GetAwaiter().GetResult();
            Assert.True(updateResult.Success);

            //waiting for event bus execution
            Thread.Sleep(3000);

            musicians.ForEach(musician =>
            {
                var updatedMusician = _baseQueryRepository.GetAsync<Musician>(musician.Login).GetAwaiter().GetResult();
                Assert.Contains(band.Login, updatedMusician.BandLogins);
            });
        }

        private Musician GetMusician()
        {
            return new Musician
            {
                Name = "qwerty",
                Address = "Spb",
                BirthDate = DateTime.Now.AddYears(-20),
                Description = "qweqweqwe",
                Login = Guid.NewGuid().ToString(),
                Experience = Experiences.Beginer,
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
