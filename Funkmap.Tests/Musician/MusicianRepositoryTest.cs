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
    public class MusicianRepositoryTest
    {
        private MusicianRepository _musicianRepository;

        [TestInitialize]
        public void Initialize()
        {
            _musicianRepository = new MusicianRepository(new FakeMusicianDbContext());
        }

        [TestMethod]
        public void FindNearest()
        {

            var result = _musicianRepository.GetNearestMusicianPreviews(null).Result;
            Assert.AreEqual(result.Count, 3);

            var parameters = new LocationParameter()
            {
                Longitude = 30,
                Latitude = 50,
                RadiusDeg = 0.5
            };

            result = _musicianRepository.GetNearestMusicianPreviews(parameters).Result;
            Assert.AreEqual(result.Count, 1);

        }

        [TestMethod]
        public void GetStyleFilteredMusicianTest()
        {

            var parameter = new MusicianParameter()
            {
                Styles = new List<Styles>() { Styles.Funk }
            };

            var result = _musicianRepository.GetFiltered(parameter).Result;
            Assert.AreEqual(result.Count, 2);

            parameter.Styles.Add(Styles.Rock);
            result = _musicianRepository.GetFiltered(parameter).Result;
            Assert.AreEqual(result.Count, 1);

            parameter.Styles.Add(Styles.HipHop);
            result = _musicianRepository.GetFiltered(parameter).Result;
            Assert.AreEqual(result.Count, 0);


        }

        [TestMethod]
        public void StylesToArray()
        {
            var stylesEnum = Styles.Funk | Styles.HipHop;
            var stylesResult = Enum.GetValues(typeof(Styles)).Cast<Styles>().Where(allStyles => (stylesEnum & allStyles) != 0).ToList();
            stylesResult.Sort();
            var styles = new List<Styles>() { Styles.Funk, Styles.HipHop };
            styles.Sort();

            Assert.AreEqual(stylesResult.Count, styles.Count);

            for (int i = 0; i < styles.Count; i++)
            {
                Assert.AreEqual(stylesResult[i], styles[i]);
            }
        }
    }
}
