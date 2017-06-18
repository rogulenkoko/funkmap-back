using Funkmap.Auth.Data;
using Funkmap.Tests.Funkmap.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Funkmap.Tests.Funkmap.Auth
{
    [TestClass]
    public class AuthRepositoryTest
    {
        [TestMethod]
        public void LoginTest()
        {
            var repository = new AuthRepository(new FakeAuthDbContext());
            var result = repository.Login("rogulenkoko", "1").Result;
            Assert.IsNotNull(result);

            result = repository.Login("rogulenkoko", "2").Result;
            Assert.IsNull(result);

            result = repository.Login("test", "2").Result;
            Assert.IsNull(result);

            
        }
    }
}
