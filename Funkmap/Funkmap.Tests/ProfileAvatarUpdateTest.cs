using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Funkmap.Common.Abstract;
using Funkmap.Common.Core.Tools;
using Funkmap.Common.Models;
using Funkmap.Cqrs.Abstract;
using Funkmap.Data;
using Funkmap.Data.Entities.Entities.Abstract;
using Funkmap.Data.Repositories;
using Funkmap.Data.Services;
using Funkmap.Data.Services.Abstract;
using Funkmap.Data.Tools;
using Funkmap.Domain.Abstract.Repositories;
using Funkmap.Domain.Parameters;
using Funkmap.Tests.Data;
using Funkmap.Tests.Images;
using Funkmap.Tests.Tools;
using MongoDB.Driver;
using Moq;
using Xunit;

namespace Funkmap.Tests
{
    public class ProfileAvatarUpdateTest
    {
        private readonly IBaseQueryRepository _baseQueryRepository;
        private readonly TestToolRepository _toolRepository;
        private readonly IMongoCollection<BaseEntity> _collection;

        public ProfileAvatarUpdateTest()
        {
            var db = FunkmapTestDbProvider.DropAndCreateDatabase();

            var storage = new Mock<IFileStorage>().Object;

            var filterServices = new List<IFilterService>() { new MusicianFilterService() };
            IFilterFactory factory = new FilterFactory(filterServices);

            _collection = db.GetCollection<BaseEntity>(CollectionNameProvider.BaseCollectionName);
            
            _baseQueryRepository = new BaseQueryRepository(_collection, storage, factory);

            _toolRepository = new TestToolRepository(_collection);
        }

        [Fact]
        public void UpdateAvatarTest()
        {
            var profileLogin = _toolRepository.GetAnyLoginsAsync(1).GetAwaiter().GetResult().SingleOrDefault();
            Assert.NotNull(profileLogin);

            var profile = _baseQueryRepository.GetAsync(profileLogin).GetAwaiter().GetResult();

            var imageBytes = ImageProvider.GetAvatar("avatar.jpg");

            var parameter = new CommandParameter<AvatarUpdateParameter>()
            {
                UserLogin = profile.UserLogin,
                Parameter = new AvatarUpdateParameter()
                {
                    Login = profileLogin,
                    Bytes = imageBytes
                }
            };

            var defaultOptions = new ImageProcessorOptions();

            using (var mock = AutoMock.GetLoose())
            {
                var fileName = ImageNameBuilder.BuildAvatarName(profileLogin);
                var fileBytes = FunkmapImageProcessor.MinifyImage(imageBytes, defaultOptions.Size);
                var expectedName = $"{fileName}_uploaded";
                mock.Mock<IFileStorage>().Setup(x => x.UploadFromBytesAsync(fileName, fileBytes))
                    .Returns(Task.FromResult(expectedName));

                var fileMiniName = ImageNameBuilder.BuildAvatarMiniName(profileLogin);
                var fileMiniBytes = FunkmapImageProcessor.MinifyImage(imageBytes, defaultOptions.MiniSize);

                var expectedMiniName = $"{fileName}_uploaded_mini";
                mock.Mock<IFileStorage>().Setup(x => x.UploadFromBytesAsync(fileMiniName, fileMiniBytes))
                    .Returns(Task.FromResult(expectedMiniName));

                var storage = mock.Create<IFileStorage>();
                var eventBus = new Mock<IEventBus>().Object;
                var commandRepository = new BaseCommandRepository(_collection, storage, new List<IUpdateDefinitionBuilder>(), eventBus);
                var result = commandRepository.UpdateAvatarAsync(parameter).GetAwaiter().GetResult();
                Assert.True(result.Success);

                var updatedProfile = _baseQueryRepository.GetAsync(profileLogin).GetAwaiter().GetResult();
                Assert.NotNull(updatedProfile);
                Assert.Equal(updatedProfile.AvatarUrl, expectedName);
                Assert.Equal(updatedProfile.AvatarMiniUrl, expectedMiniName);

            }
        }

        [Fact]
        public void UpdateEmptyAvatarTest()
        {
            var profileLogin = _toolRepository.GetAnyLoginsAsync(1).GetAwaiter().GetResult().SingleOrDefault();
            Assert.NotNull(profileLogin);

            var profile = _baseQueryRepository.GetAsync(profileLogin).GetAwaiter().GetResult();
            
            var parameter = new CommandParameter<AvatarUpdateParameter>()
            {
                UserLogin = profile.UserLogin,
                Parameter = new AvatarUpdateParameter()
                {
                    Login = profileLogin,
                    Bytes = new byte []{}
                }
            };
            
            IFileStorage storage = new Mock<IFileStorage>().Object;
            var eventBus = new Mock<IEventBus>().Object;
            var commandRepository = new BaseCommandRepository(_collection, storage, new List<IUpdateDefinitionBuilder>(), eventBus);
            var result = commandRepository.UpdateAvatarAsync(parameter).GetAwaiter().GetResult();
            Assert.True(result.Success);

            var updatedProfile = _baseQueryRepository.GetAsync(profileLogin).GetAwaiter().GetResult();
            Assert.NotNull(updatedProfile);
            Assert.Equal(updatedProfile.AvatarUrl, String.Empty);
            Assert.Equal(updatedProfile.AvatarMiniUrl, String.Empty);
        }
    }
}
