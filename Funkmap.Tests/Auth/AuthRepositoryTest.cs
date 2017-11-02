using System.IO;
using Funkmap.Auth.Data;
using Funkmap.Auth.Data.Abstract;
using Funkmap.Auth.Data.Entities;
using Funkmap.Module.Auth;
using Funkmap.Tests.Auth.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Funkmap.Tests.Auth
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
        public void GetUserByEmailTest()
        {
            string path = @"${basedir}";
            var der = Directory.GetDirectories(path);
            UserEntity user = _repository.GetUserByEmail("timoshka_kirov@mail.ru").Result;
            Assert.AreEqual(user.Password, "123");
        }
    }
}