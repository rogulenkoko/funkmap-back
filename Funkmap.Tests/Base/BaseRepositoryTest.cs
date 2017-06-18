using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Common;
using Funkmap.Data.Entities;
using Funkmap.Data.Entities.Abstract;
using Funkmap.Data.Repositories;
using Funkmap.Tests.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using MongoDB.Driver.GeoJsonObjectModel;

namespace Funkmap.Tests.Mongo
{
    [TestClass]
    public class EntityRepositoryTest
    {

        private IMongoCollection<BaseEntity> _baseCollection;

        [TestInitialize]
        public void Initialize()
        {
            _baseCollection = DbProvider.DropAndCreateDatabase.GetCollection<BaseEntity>(CollectionNameProvider.BaseCollectionName);

        }

        [TestMethod]
        public void GetAll()
        {
            var all = new BaseRepository(_baseCollection).GetAllAsyns().Result;

        }
    }


}