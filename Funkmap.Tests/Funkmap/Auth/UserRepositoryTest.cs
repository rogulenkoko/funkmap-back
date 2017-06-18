using Funkmap.Auth.Data;
using Funkmap.Tests.Funkmap.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Funkmap.Tests.Funkmap.Auth
{
    [TestClass]
    public class UserRepositoryTest
    {
        [TestMethod]
        public void GetAvatarTest()
        {
            var repository = new AuthRepository(new FakeAuthDbContext());
            var result = repository.GetAvatarAsync("rogulenkoko").Result;
        }
    }
}
