using System.Collections.Generic;
using Funkmap.Data.Domain;
using Funkmap.Data.Entities;
using Funkmap.Data.Parameters;
using Funkmap.Data.Repositories;
using Funkmap.Data.Repositories.Abstract;
using Funkmap.Tests.Funkmap.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Funkmap.Tests.Funkmap.Base
{
    [TestClass]
    public class EntityRepositoryTest
    {

        private IBaseRepository _baseRepository;

        [TestInitialize]
        public void Initialize()
        {
            _baseRepository = new BaseRepository(FunkmapDbProvider.DropAndCreateDatabase.GetCollection<BaseEntity>(CollectionNameProvider.BaseCollectionName));

        }

        [TestMethod]
        public void GetAll()
        {
            var all = _baseRepository.GetAllAsyns().Result;
            Assert.AreEqual(all.Count, 10);
        }

        [TestMethod]
        public void GetNearestTest()
        {
            var parameter = new LocationParameter()
            {
                Longitude = 30,
                Latitude = 50,
                RadiusDeg = 2
            };
            var nearest = _baseRepository.GetNearestAsync(parameter).Result;
            Assert.AreEqual(nearest.Count, 5);
        }


        [TestMethod]
        public void GetSpecificTest()
        {
            var logins = new List<string>() { "razrab" };
            var result = _baseRepository.GetSpecificAsync(logins.ToArray()).Result;
            Assert.AreEqual(result.Count, 1);

            logins.Add("rogulenkoko");
            result = _baseRepository.GetSpecificAsync(logins.ToArray()).Result;
            Assert.AreEqual(result.Count, 2);
        }
    }
}