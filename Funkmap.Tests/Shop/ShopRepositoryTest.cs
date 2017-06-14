using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Common.Data.Parameters;
using Funkmap.Shop.Data.Repositories;
using Funkmap.Tests.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Funkmap.Tests.Shop
{
    [TestClass]
    public class ShopRepositoryTest
    {
        [TestMethod]
        public void GetNearestShopTest()
        {
            var shopRepositry = new ShopRepository(new FakeShopDbContext());

            var results = shopRepositry.GetShopsPreviews().Result;
            Assert.IsNotNull(results);
            Assert.AreEqual(results.Count,4);

            results = shopRepositry.GetNearestShopsPreviews(null).Result;
            Assert.IsNotNull(results);
            Assert.AreEqual(results.Count, 4);

            var locParameter = new LocationParameter()
            {
                Longitude = 31,
                Latitude = 53,
                RadiusDeg = 1
            };
            results = shopRepositry.GetNearestShopsPreviews(locParameter).Result;
            Assert.IsNotNull(results);
            Assert.AreEqual(results.Count, 2);

            locParameter.RadiusDeg = 0;
            results = shopRepositry.GetNearestShopsPreviews(locParameter).Result;
            Assert.IsNotNull(results);
            Assert.AreEqual(results.Count, 0);

            results = shopRepositry.GetShopsPreviewsSearchByName("один").Result;
            Assert.IsNotNull(results);
            Assert.AreEqual(results.Count, 1);

            results = shopRepositry.GetShopsPreviewsSearchByName("адин").Result;
            Assert.IsNotNull(results);
            Assert.AreEqual(results.Count, 0);
        }
    }
}
