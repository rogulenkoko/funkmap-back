using System;
using Funkmap.Data;
using Funkmap.Data.Entities.Entities;
using Funkmap.Data.Repositories;
using Funkmap.Domain.Abstract.Repositories;
using Funkmap.Tests.Funkmap.Data;
using Funkmap.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Funkmap.Tests.Funkmap.Musician
{
    [TestClass]
    public class MusicianRepositoryTest
    {
        [TestInitialize]
        public void Initialize()
        {
            
        }
        
        [TestMethod]
        public void UpdateEntityExtensionTest()
        {
            var entity = new MusicianEntity()
            {
                Login = "asd",
                Description = "aaaaa"
            };

            var newEntity = new MusicianEntity()
            {
                Login = "asd",
                BirthDate = DateTime.Now,
                Description = "zzzzz"
            };

            var t = entity.FillEntity<MusicianEntity>(newEntity);

            Assert.AreEqual(entity.Login, newEntity.Login);

            Assert.AreEqual(entity.BirthDate, newEntity.BirthDate);
            Assert.AreEqual(entity.Description, newEntity.Description);

        }
    }
}
