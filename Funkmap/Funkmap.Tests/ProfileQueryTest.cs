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
using Funkmap.Tests.Data;
using Funkmap.Tests.Tools;
using Moq;
using Xunit;

namespace Funkmap.Tests
{
    public class ProfileQueryTest
    {
        private readonly IBaseQueryRepository _baseQueryRepository;
        private readonly TestToolRepository _toolRepository;

        public ProfileQueryTest()
        {
            var db = FunkmapTestDbProvider.DropAndCreateDatabase();

            var storage = new Mock<IFileStorage>().Object;

            var filterServices = new List<IFilterService>() { new MusicianFilterService() };
            IFilterFactory factory = new FilterFactory(filterServices);

            var collection = db.GetCollection<BaseEntity>(CollectionNameProvider.BaseCollectionName);

            _baseQueryRepository = new BaseQueryRepository(collection, storage, factory);

            _toolRepository = new TestToolRepository(collection);
        }

        [Fact]
        public void GetProfileTest()
        {
            var logins = _toolRepository.GetAnyLoginsAsync().GetAwaiter().GetResult();

            foreach (var login in logins)
            {
                Profile profile = _baseQueryRepository.GetAsync(login).GetAwaiter().GetResult();

                Assert.Equal(login, profile.Login);
                Assert.False(String.IsNullOrEmpty(profile.Name));
                Assert.False(String.IsNullOrEmpty(profile.UserLogin));
                Assert.False(profile.EntityType == 0);
            }
        }

        [Fact]
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

            List<Marker> nearest = _baseQueryRepository.GetNearestMarkersAsync(parameter).GetAwaiter().GetResult();

            Assert.True(nearest.Count < parameter.Take);

            var allDistances = _toolRepository.GetDistances(parameter).Results.ToDictionary(x => x.Profile.Login, y => y.Distance * FunkmapConstants.EarthRadius);

            var isValid = nearest.All(x => allDistances[x.Login] <= parameter.RadiusKm);

            Assert.True(isValid);

            parameter.RadiusKm = null;

            List<Marker> allNearest = _baseQueryRepository.GetNearestMarkersAsync(parameter).GetAwaiter().GetResult();

            Assert.True(allNearest.Count > 0 && allNearest.Count >= nearest.Count && allNearest.Count < parameter.Take);

            Assert.Equal(allNearest.Select(x => x.Login).Intersect(nearest.Select(x => x.Login)).Count(), nearest.Count);

            Assert.True(allNearest.All(x => x.ModelType != 0 && !String.IsNullOrEmpty(x.Login)));

            Assert.Throws<ArgumentException>(() => _baseQueryRepository.GetNearestMarkersAsync(null).GetAwaiter().GetResult());
        }

        [Fact]
        public void GetSpecificNavigationTest()
        {
            var result = _baseQueryRepository.GetSpecificMarkersAsync(null).GetAwaiter().GetResult();
            Assert.Equal(result.Count, 0);

            var logins = new List<string>();
            result = _baseQueryRepository.GetSpecificMarkersAsync(logins).GetAwaiter().GetResult();
            Assert.Equal(result.Count, 0);

            var anyLogins = _toolRepository.GetAnyLoginsAsync(2).GetAwaiter().GetResult();

            Assert.NotEqual(anyLogins.Count, 0);

            logins.Add(anyLogins.First());

            result = _baseQueryRepository.GetSpecificMarkersAsync(logins).GetAwaiter().GetResult();
            Assert.Equal(result.Count, 1);

            logins.Add(anyLogins.Last());
            result = _baseQueryRepository.GetSpecificMarkersAsync(logins).GetAwaiter().GetResult();
            Assert.Equal(result.Count, anyLogins.Count);
        }

        [Fact]
        public void GetFilteredTest()
        {
            var commonParameter = new CommonFilterParameter
            {
                EntityType = EntityType.Musician
            };

            List<Profile> result = _baseQueryRepository.GetFilteredAsync(commonParameter).GetAwaiter().GetResult();
            var count = _baseQueryRepository.GetAllFilteredCountAsync(commonParameter).GetAwaiter().GetResult();

            Assert.Equal(result.Count, count);

            Assert.True(result.All(x => x.EntityType == EntityType.Musician));

            var musicianParameter = new MusicianFilterParameter
            {
                Styles = new List<Styles>() { Styles.HipHop },
                Expiriences = new List<Expiriences>() { Expiriences.Advanced }
            };

            result = _baseQueryRepository.GetFilteredAsync(commonParameter, musicianParameter).GetAwaiter().GetResult();
            count = _baseQueryRepository.GetAllFilteredCountAsync(commonParameter, musicianParameter).GetAwaiter().GetResult();

            Assert.Equal(result.Count, count);

            Assert.True(result.All(x => x.EntityType == EntityType.Musician));

            Assert.True(result.Select(x => x as Domain.Models.Musician).All(x => x.Styles.Intersect(musicianParameter.Styles).Any()));
            Assert.True(result.Select(x => x as Domain.Models.Musician).All(x => musicianParameter.Expiriences.Contains(x.Experience)));

            commonParameter = new CommonFilterParameter()
            {
                Latitude = 50,
                Longitude = 30,
                Take = 100
            };

            result = _baseQueryRepository.GetFilteredAsync(commonParameter).GetAwaiter().GetResult();
            count = _baseQueryRepository.GetAllFilteredCountAsync(commonParameter).GetAwaiter().GetResult();

            Assert.Equal(result.Count, count);

            Assert.NotEqual(result.Count, 0);

            Assert.True(result.All(x => x.EntityType != 0));
            Assert.True(result.All(x => !String.IsNullOrEmpty(x.Login)));
            Assert.True(result.All(x => !String.IsNullOrEmpty(x.Name)));
            Assert.True(result.All(x => !String.IsNullOrEmpty(x.UserLogin)));

            Assert.True(result.Where(x => x.EntityType == EntityType.Musician).All(x => (x as Domain.Models.Musician).Instrument != Instruments.None));

            var allDistances = _toolRepository.GetDistances(commonParameter).Results.ToDictionary(x => x.Profile.Login, y => y.Distance * FunkmapConstants.EarthRadius);

            var distancesPairs = allDistances.Zip(allDistances.Skip(1), Tuple.Create);

            var areOrdered = distancesPairs.All(x => x.Item1.Value <= x.Item2.Value);
            Assert.True(areOrdered);
        }

        [Fact]
        public void GetUsersEntitiesCountTest()
        {
            var profileUsers = _toolRepository.GetProfileUsersAsync().GetAwaiter().GetResult();
            Assert.NotEqual(profileUsers.Count, 0);

            foreach (var profileUser in profileUsers)
            {
                List<UserEntitiesCountInfo> result = _baseQueryRepository.GetUserEntitiesCountInfoAsync(profileUser).GetAwaiter().GetResult();
                Assert.NotEqual(result.Count, 0);

                Assert.True(result.SelectMany(x => x.Logins).Any());
                Assert.True(result.Select(x => x.Count).Aggregate((x, y) => x + y) > 0);
                Assert.True(result.All(x => x.Count == x.Logins.Count));
                Assert.True(result.All(x => x.EntityType != 0));
            }
        }

        [Fact]
        public void GetUserEntitiesLoginsTest()
        {
            var profileUsers = _toolRepository.GetProfileUsersAsync().GetAwaiter().GetResult();

            foreach (var profileUser in profileUsers)
            {
                List<string> profilesLogins = _baseQueryRepository.GetUserEntitiesLoginsAsync(profileUser).GetAwaiter().GetResult();
                Assert.NotEqual(profilesLogins.Count, 0);
                Assert.True(profilesLogins.All(x => !String.IsNullOrEmpty(x)));
            }
        }

        [Fact]
        public void LoginExistsTest()
        {
            var logins = _toolRepository.GetAnyLoginsAsync().GetAwaiter().GetResult();

            foreach (var login in logins)
            {
                var isExist = _baseQueryRepository.LoginExistsAsync(login).GetAwaiter().GetResult();
                Assert.True(isExist);
            }

            var exist = _baseQueryRepository.LoginExistsAsync(String.Empty).GetAwaiter().GetResult();
            Assert.False(exist);

            exist = _baseQueryRepository.LoginExistsAsync(Guid.NewGuid().ToString()).GetAwaiter().GetResult();
            Assert.False(exist);
        }
    }
}