using System;
using System.Collections.Generic;
using System.Linq;
using Funkmap.Common.Abstract;
using Funkmap.Common.Cqrs.Abstract;
using Funkmap.Data;
using Funkmap.Data.Entities.Entities.Abstract;
using Funkmap.Data.Repositories;
using Funkmap.Data.Services;
using Funkmap.Data.Services.Abstract;
using Funkmap.Data.Services.Update;
using Funkmap.Domain.Abstract.Repositories;
using Funkmap.Domain.Parameters;
using Funkmap.Tests.Data;
using Funkmap.Tests.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Funkmap.Tests
{
    [TestClass]
    public class FavoriteProfilesTest
    {
        private IBaseCommandRepository _commandRepository;
        private IBaseQueryRepository _baseQueryRepository;

        private TestToolRepository _toolRepository;

        [TestInitialize]
        public void Initialize()
        {
            var db = FunkmapTestDbProvider.DropAndCreateDatabase();

            var storage = new Mock<IFileStorage>().Object;

            var collection = db.GetCollection<BaseEntity>(CollectionNameProvider.BaseCollectionName);

            var updateBuilders = new List<IUpdateDefenitionBuilder>() { new MusicianUpdateDefenitionBuilder(), new BandUpdateDefenitionBuilder(), new ShopUpdateDefenitionBuilder() };
            
            var eventBus = new Mock<IEventBus>().Object;

            _commandRepository = new BaseCommandRepository(collection, storage, updateBuilders, eventBus);

            var filterServices = new List<IFilterService>() { new MusicianFilterService() };
            IFilterFactory factory = new FilterFactory(filterServices);

            _baseQueryRepository = new BaseQueryRepository(collection, storage, factory);

            _toolRepository = new TestToolRepository(collection);
            
        }

        [TestMethod]
        public void UpdateFavouriteTest()
        {
            var someUser = Guid.NewGuid().ToString();

            var profileLogin = _toolRepository.GetAnyLoginsAsync(1).GetAwaiter().GetResult().SingleOrDefault();
            Assert.IsNotNull(profileLogin);
            
            var parameter = new UpdateFavoriteParameter
            {
                UserLogin = someUser,
                IsFavorite = true,
                ProfileLogin = profileLogin
            };

            var response = _commandRepository.UpdateFavoriteAsync(parameter).GetAwaiter().GetResult();
            Assert.IsTrue(response.Success);

            var favoriteLogins = _baseQueryRepository.GetFavoritesLoginsAsync(someUser).GetAwaiter().GetResult();
            Assert.IsTrue(favoriteLogins.Contains(profileLogin));

            response = _commandRepository.UpdateFavoriteAsync(parameter).GetAwaiter().GetResult();
            Assert.IsFalse(response.Success);

            var someUser1 = Guid.NewGuid().ToString();
            parameter.UserLogin = someUser1;

            response = _commandRepository.UpdateFavoriteAsync(parameter).GetAwaiter().GetResult();
            Assert.IsTrue(response.Success);

            favoriteLogins = _baseQueryRepository.GetFavoritesLoginsAsync(someUser).GetAwaiter().GetResult();
            Assert.IsTrue(favoriteLogins.Contains(profileLogin));

            favoriteLogins = _baseQueryRepository.GetFavoritesLoginsAsync(someUser1).GetAwaiter().GetResult();
            Assert.IsTrue(favoriteLogins.Contains(profileLogin));

            parameter.IsFavorite = false;

            response = _commandRepository.UpdateFavoriteAsync(parameter).GetAwaiter().GetResult();
            Assert.IsTrue(response.Success);

            favoriteLogins = _baseQueryRepository.GetFavoritesLoginsAsync(someUser1).GetAwaiter().GetResult();
            Assert.IsFalse(favoriteLogins.Contains(profileLogin));


        }
    }
}
