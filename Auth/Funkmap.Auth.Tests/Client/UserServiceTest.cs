using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Funkmap.Auth.Tests.Client
{
    [TestClass]
    public class UserServiceTest
    {
        [TestMethod]
        public async Task GetUserTest()
        {
            var url = "http://bandmap-api.azurewebsites.net";
            var userService = new UserService(url);
            var userLogin = "rogulenkoko";
            var user = await userService.GetUserAsync(userLogin);
            Assert.IsNotNull(user);
            Assert.IsTrue(user.IsExists);
            Assert.AreEqual(user.User.Login, userLogin);
        } 
    }
}
