using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Funkmap.Common.Data.Mongo.Entities;
using Funkmap.Data;
using Funkmap.Data.Entities;
using Funkmap.Data.Entities.Abstract;
using Funkmap.Data.Parameters;
using Funkmap.Data.Repositories;
using Funkmap.Data.Repositories.Abstract;
using Funkmap.Models;
using Funkmap.Tests.Funkmap.Data;
using Funkmap.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;

namespace Funkmap.Tests.Funkmap.Musician
{
    [TestClass]
    public class MusicianRepositoryTest
    {
        private IMusicianRepository _musicianRepository;

        [TestInitialize]
        public void Initialize()
        {
            _musicianRepository = new MusicianRepository(FunkmapTestDbProvider.DropAndCreateDatabase.GetCollection<MusicianEntity>(CollectionNameProvider.BaseCollectionName));
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
                Description = "zzzzz",
                Photo = new ImageInfo() {Image = new byte[] { 1, 2, 3 } } 
            };

            var t = entity.FillEntity<MusicianEntity>(newEntity);

            Assert.AreEqual(entity.Login, newEntity.Login);

            Assert.AreEqual(entity.BirthDate, newEntity.BirthDate);
            Assert.AreEqual(entity.Description, newEntity.Description);

        }
    }
}
