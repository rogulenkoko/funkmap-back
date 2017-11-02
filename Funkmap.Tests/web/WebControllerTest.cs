using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Funkmap.Tests.web
{
    [TestClass]
    public class WebControllerTest
    {
        private string _testServerUri = "http://127.0.0.1:9000/api";
        private WebTestServise _webTestServise;
        private string _token;


        [TestInitialize]
        public void init()
        {
            _webTestServise = new WebTestServise(_testServerUri);
           _token= _webTestServise.Login();

        }

        [TestMethod]
        public void controllerTestGetBandById()
        {
            var band = _webTestServise.GetBandModel(0);
            Assert.IsNotNull(band);
        }
    }
}
