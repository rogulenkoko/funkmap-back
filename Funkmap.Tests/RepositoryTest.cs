using System;
using System.Linq;
using Funkmap.Module.Musician.Data;
using Funkmap.Tests.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Funkmap.Tests
{
    [TestClass]
    public class RepositoryTest
    {
        [TestMethod]
        public void MusicianRepositooryTest()
        {
            var context = new FakeMusicianDbContext();

            var repository = new MusicianRepository(context);

            var musicians = repository.GetAllAsync().Result.ToList();
        }
    }
}
