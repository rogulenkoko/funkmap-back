using Funkmap.Data.Entities.Abstract;
using Funkmap.Data.Repositories;
using Funkmap.Tests.Funkmap.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;

namespace Funkmap.Tests.Funkmap.Base
{
    [TestClass]
    public class EntityRepositoryTest
    {

        private IMongoCollection<BaseEntity> _baseCollection;

        [TestInitialize]
        public void Initialize()
        {
            _baseCollection = FunkmapDbProvider.DropAndCreateDatabase.GetCollection<BaseEntity>(CollectionNameProvider.BaseCollectionName);

        }

        [TestMethod]
        public void GetAll()
        {
            var all = new BaseRepository(_baseCollection).GetAllAsyns().Result;
            Assert.AreEqual(all.Count,10);
        }
    }


}