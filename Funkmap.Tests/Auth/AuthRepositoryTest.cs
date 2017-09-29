using System.IO;
using System.Threading.Tasks;
using Funkmap.Auth.Data;
using Funkmap.Auth.Data.Abstract;
using Funkmap.Auth.Data.Entities;
using Funkmap.Tests.Funkmap.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Funkmap.Module.Auth;
using Funkmap.Tests.Auth.Data;

namespace Funkmap.Tests.Funkmap.Auth
{
    [TestClass]
    public class AuthRepositoryTest
    {
        private IAuthRepository _repository;

        [TestInitialize]
        public void Initialize()
        {
            _repository =
                new AuthRepository(
                    AuthTestDbProvider.DropAndCreateDatabase.GetCollection<UserEntity>(AuthCollectionNameProvider
                        .UsersCollectionName));
        }

        [TestMethod]
        public void LoginTest()
        {
            var result = _repository.Login("rogulenkoko", "1").Result;
            Assert.IsNotNull(result);

            result = _repository.Login("rogulenkoko", "2").Result;
            Assert.IsNull(result);

            result = _repository.Login("test", "2").Result;
            Assert.IsNull(result);
        }

        [TestMethod]
        public void FavouritesTest()
        {
            _repository.SetFavourite("test", "razrab").Wait();
            var favourites = _repository.GetFavouritesAsync("test").Result;
            Assert.AreEqual(favourites.Count, 2);

            _repository.SetFavourite("test", "razrab").Wait();
            favourites = _repository.GetFavouritesAsync("test").Result;
            Assert.AreEqual(favourites.Count, 1);
        }

        [TestMethod]
        public void GetUserByEmailTest()
        {
            string path = @"${basedir}";
            var der = Directory.GetDirectories(path);
            UserEntity user = _repository.GetUserByEmail("timoshka_kirov@mail.ru").Result;
            Assert.AreEqual(user.Password, "123");
        }
    }
}