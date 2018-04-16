using Funkmap.Auth.Data;
using Funkmap.Auth.Data.Entities;
using Funkmap.Auth.Domain.Abstract;
using Funkmap.Auth.Tests.Data;
using Funkmap.Common.Abstract;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Funkmap.Auth.Tests
{
    [TestClass]
    public class AuthRepositoryTest
    {
        private IAuthRepository _repository;

        [TestInitialize]
        public void Initialize()
        {

            var fileStorage = new Mock<IFileStorage>();

            _repository =
                new AuthRepository(
                    AuthTestDbProvider.DropAndCreateDatabase.GetCollection<UserEntity>(AuthCollectionNameProvider
                        .UsersCollectionName), fileStorage.Object);
        }

        [TestMethod]
        public void LoginTest()
        {
            var login = "rogulenkoko";
            var result = _repository.LoginAsync(login, "1").GetAwaiter().GetResult();
            Assert.IsNotNull(result);
            Assert.AreEqual(login, result.Login);

            result = _repository.LoginAsync("rogulenkoko", "2").Result;
            Assert.IsNull(result);

            result = _repository.LoginAsync("test", "2").Result;
            Assert.IsNull(result);
        }
    }
}