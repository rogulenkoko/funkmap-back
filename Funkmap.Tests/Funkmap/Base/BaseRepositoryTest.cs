using System;
using System.Collections.Generic;
using System.Linq;
using Funkmap.Common.Abstract;
using Funkmap.Data;
using Funkmap.Data.Entities.Entities.Abstract;
using Funkmap.Data.Repositories;
using Funkmap.Data.Services;
using Funkmap.Data.Services.Abstract;
using Funkmap.Domain;
using Funkmap.Domain.Abstract.Repositories;
using Funkmap.Domain.Enums;
using Funkmap.Domain.Models;
using Funkmap.Domain.Parameters;
using Funkmap.Tests.Funkmap.Data;
using Funkmap.Tests.Funkmap.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Funkmap.Tests.Funkmap.Base
{
    [TestClass]
    public class EntityRepositoryTest
    {
        private IBaseRepository _baseRepository;

        private TestToolRepository _toolRepository;

        [TestInitialize]
        public void Initialize()
        {
            var db = FunkmapTestDbProvider.DropAndCreateDatabase();

            var storage = new Mock<IFileStorage>().Object;

            var filterServices = new List<IFilterService>() { new MusicianFilterService() };
            IFilterFactory factory = new FilterFactory(filterServices);

            var collection = db.GetCollection<BaseEntity>(CollectionNameProvider.BaseCollectionName);

            _baseRepository = new BaseRepository(collection, storage, factory);

            _toolRepository = new TestToolRepository(collection);
        }

        [TestMethod]
        public void GetProfileTest()
        {
            var logins = _toolRepository.GetAnyLoginsAsync().GetAwaiter().GetResult();

            foreach (var login in logins)
            {
                Profile profile = _baseRepository.GetAsync(login).GetAwaiter().GetResult();

                Assert.AreEqual(login, profile.Login);
                Assert.IsFalse(String.IsNullOrEmpty(profile.Name));
                Assert.IsFalse(String.IsNullOrEmpty(profile.UserLogin));
                Assert.IsFalse(profile.EntityType == 0);
            }
        }

        [TestMethod]
        public void GetNearestTest()
        {
            var parameter = new LocationParameter
            {
                Longitude = 30.1,
                Latitude = 50.2,
                Take = 100,
                Skip = 0,
                RadiusKm = 100
            };

            List<Marker> nearest = _baseRepository.GetNearestMarkersAsync(parameter).GetAwaiter().GetResult();

            Assert.IsTrue(nearest.Count < parameter.Take);

            var allDistances = _toolRepository.GetDistances(parameter).Results.ToDictionary(x => x.Profile.Login, y => y.Distance * FunkmapConstants.EarthRadius);

            var isValid = nearest.All(x => allDistances[x.Login] <= parameter.RadiusKm);

            Assert.IsTrue(isValid);

            parameter.RadiusKm = null;

            List<Marker> allNearest = _baseRepository.GetNearestMarkersAsync(parameter).GetAwaiter().GetResult();

            Assert.IsTrue(allNearest.Count > 0 && allNearest.Count >= nearest.Count && allNearest.Count < parameter.Take);

            Assert.AreEqual(allNearest.Select(x => x.Login).Intersect(nearest.Select(x => x.Login)).Count(), nearest.Count);

            Assert.IsTrue(allNearest.All(x => x.ModelType != 0 && !String.IsNullOrEmpty(x.Login)));

            try
            {
                _baseRepository.GetNearestMarkersAsync(null).GetAwaiter().GetResult();
                Assert.Fail();
            }
            catch (ArgumentException)
            {
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void GetSpecificNavigationTest()
        {
            var result = _baseRepository.GetSpecificMarkersAsync(null).GetAwaiter().GetResult();
            Assert.AreEqual(result.Count, 0);

            var logins = new List<string>();
            result = _baseRepository.GetSpecificMarkersAsync(logins).GetAwaiter().GetResult();
            Assert.AreEqual(result.Count, 0);

            var anyLogins = _toolRepository.GetAnyLoginsAsync(2).GetAwaiter().GetResult();

            Assert.AreNotEqual(anyLogins.Count, 0);

            logins.Add(anyLogins.First());

            result = _baseRepository.GetSpecificMarkersAsync(logins).GetAwaiter().GetResult();
            Assert.AreEqual(result.Count, 1);

            logins.Add(anyLogins.Last());
            result = _baseRepository.GetSpecificMarkersAsync(logins).GetAwaiter().GetResult();
            Assert.AreEqual(result.Count, anyLogins.Count);
        }

        [TestMethod]
        public void GetFilteredTest()
        {
            var commonParameter = new CommonFilterParameter
            {
                EntityType = EntityType.Musician
            };

            List<Profile> result = _baseRepository.GetFilteredAsync(commonParameter).GetAwaiter().GetResult();
            var count = _baseRepository.GetAllFilteredCountAsync(commonParameter).GetAwaiter().GetResult();

            Assert.AreEqual(result.Count, count);

            Assert.IsTrue(result.All(x => x.EntityType == EntityType.Musician));

            var musicianParameter = new MusicianFilterParameter
            {
                Styles = new List<Styles>() { Styles.HipHop },
                Expiriences = new List<Expiriences>() { Expiriences.Advanced }
            };

            result = _baseRepository.GetFilteredAsync(commonParameter, musicianParameter).GetAwaiter().GetResult();
            count = _baseRepository.GetAllFilteredCountAsync(commonParameter, musicianParameter).GetAwaiter().GetResult();

            Assert.AreEqual(result.Count, count);

            Assert.IsTrue(result.All(x => x.EntityType == EntityType.Musician));

            Assert.IsTrue(result.Select(x => x as Domain.Models.Musician).All(x => x.Styles.Intersect(musicianParameter.Styles).Any()));
            Assert.IsTrue(result.Select(x => x as Domain.Models.Musician).All(x => musicianParameter.Expiriences.Contains(x.Expirience)));

            commonParameter = new CommonFilterParameter()
            {
                Latitude = 50,
                Longitude = 30,
                Take = 100
            };

            result = _baseRepository.GetFilteredAsync(commonParameter).GetAwaiter().GetResult();
            count = _baseRepository.GetAllFilteredCountAsync(commonParameter).GetAwaiter().GetResult();

            Assert.AreEqual(result.Count, count);

            Assert.AreNotEqual(result.Count, 0);

            Assert.IsTrue(result.All(x => x.EntityType != 0));
            Assert.IsTrue(result.All(x => !String.IsNullOrEmpty(x.Login)));
            Assert.IsTrue(result.All(x => !String.IsNullOrEmpty(x.Name)));
            Assert.IsTrue(result.All(x => !String.IsNullOrEmpty(x.UserLogin)));

            Assert.IsTrue(result.Where(x => x.EntityType == EntityType.Musician).All(x => (x as Domain.Models.Musician).Instrument != Instruments.None));

            var allDistances = _toolRepository.GetDistances(commonParameter).Results.ToDictionary(x => x.Profile.Login, y => y.Distance * FunkmapConstants.EarthRadius);

            var distancesPairs = allDistances.Zip(allDistances.Skip(1), Tuple.Create);

            var areOrdered = distancesPairs.All(x => x.Item1.Value <= x.Item2.Value);
            Assert.IsTrue(areOrdered);
        }

        [TestMethod]
        public void GetUsersEntitiesCountTest()
        {
            var profileUsers = _toolRepository.GetProfileUsersAsync().GetAwaiter().GetResult();
            Assert.AreNotEqual(profileUsers.Count, 0);

            foreach (var profileUser in profileUsers)
            {
                List<UserEntitiesCountInfo> result = _baseRepository.GetUserEntitiesCountInfoAsync(profileUser).GetAwaiter().GetResult();
                Assert.AreNotEqual(result.Count, 0);

                Assert.IsTrue(result.SelectMany(x => x.Logins).Any());
                Assert.IsTrue(result.Select(x => x.Count).Aggregate((x, y) => x + y) > 0);
                Assert.IsTrue(result.All(x => x.Count == x.Logins.Count));
                Assert.IsTrue(result.All(x => x.EntityType != 0));
            }
        }

        [TestMethod]
        public void GetUserEntitiesLoginsTest()
        {
            var profileUsers = _toolRepository.GetProfileUsersAsync().GetAwaiter().GetResult();

            foreach (var profileUser in profileUsers)
            {
                List<string> profilesLogins = _baseRepository.GetUserEntitiesLoginsAsync(profileUser).GetAwaiter().GetResult();
                Assert.AreNotEqual(profilesLogins.Count, 0);
                Assert.IsTrue(profilesLogins.All(x => !String.IsNullOrEmpty(x)));
            }
        }

        [TestMethod]
        public void LoginExistsTest()
        {
            var logins = _toolRepository.GetAnyLoginsAsync().GetAwaiter().GetResult();

            foreach (var login in logins)
            {
                var isExist = _baseRepository.LoginExistsAsync(login).GetAwaiter().GetResult();
                Assert.IsTrue(isExist);
            }

            var exist = _baseRepository.LoginExistsAsync(String.Empty).GetAwaiter().GetResult();
            Assert.IsFalse(exist);

            exist = _baseRepository.LoginExistsAsync(Guid.NewGuid().ToString()).GetAwaiter().GetResult();
            Assert.IsFalse(exist);
        }
    }
}