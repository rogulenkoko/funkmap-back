using System;
using System.Collections.Generic;
using System.Linq;
using Funkmap.Common.Data.Parameters;
using Funkmap.Musician.Data;
using Funkmap.Musician.Data.Entities;
using Funkmap.Musician.Data.Parameters;
using Funkmap.Musician.Data.Repositories;
using Funkmap.Tests.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Funkmap.Tests.Musician
{
    [TestClass]
    public class BandRepositoryTest
    {
        [TestMethod]
        public void GetNearestBandsTest()
        {
            var bandRepository = new BandRepository(new FakeMusicianDbContext());

            var results = bandRepository.GetNearestBandsPreviews(null).Result;
            Assert.IsNotNull(results);
            Assert.AreEqual(results.Count, 3);

            var parameters = new LocationParameter()
            {
                Longitude = 50,
                Latitude = 30,
                RadiusDeg = 1
            };

            results = bandRepository.GetNearestBandsPreviews(parameters).Result;
            Assert.IsNotNull(results);
            Assert.AreEqual(results.Count, 1);



            parameters.RadiusDeg = 0;
            results = bandRepository.GetNearestBandsPreviews(parameters).Result;
            Assert.IsNotNull(results);
            Assert.AreEqual(results.Count,0);

        }
    }
}
