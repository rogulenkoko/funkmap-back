using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Common.Abstract.Search;
using Funkmap.Module.Musician.Models;
using Funkmap.Module.Search.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Funkmap.Tests.Search
{
    [TestClass]
    public class SearchTest
    {
        [TestMethod]
        public void SortByLocationTest()
        {
            var searchModels = new List<SearchModel>()
            {
                new BandSearchModel() {Longitude = 10,Latitude = 20 },
                new BandSearchModel() {Longitude = 20,Latitude = 30 },
                new BandSearchModel() {Longitude = 25,Latitude = 11 },
                new BandSearchModel() {Longitude = 2,Latitude = 50 },
                new BandSearchModel() {Longitude = 23,Latitude = 30 },
            };

            var sorted = searchModels.SortByLocationToPoint(20, 30);


        }
    }
}
