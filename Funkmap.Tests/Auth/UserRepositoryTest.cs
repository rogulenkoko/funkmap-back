using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Auth.Data;
using Funkmap.Tests.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Funkmap.Tests.Auth
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
