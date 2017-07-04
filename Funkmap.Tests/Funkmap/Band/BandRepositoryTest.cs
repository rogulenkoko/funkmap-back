using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Data.Entities;
using Funkmap.Data.Repositories;
using Funkmap.Data.Repositories.Abstract;
using Funkmap.Tests.Funkmap.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Funkmap.Tests.Funkmap.Band
{
    [TestClass]
    public class BandRepositoryTest
    {
        private IBandRepository _bandRepository;

        [TestInitialize]
        public void Initialize()
        {
            _bandRepository = new BandRepository(FunkmapTestDbProvider.DropAndCreateDatabase.GetCollection<BandEntity>(CollectionNameProvider.BaseCollectionName));
        }
        
    }
}
