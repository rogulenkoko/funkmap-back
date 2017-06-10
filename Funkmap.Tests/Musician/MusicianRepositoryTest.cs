using System;
using System.Collections.Generic;
using System.Linq;
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
        [TestMethod]
        public void GetStyleFilteredMusicianTest()
        {
            var musicianRepository = new MusicianRepository(new FakeMusicianDbContext());

            var parameter = new MusicianParameter()
            {
                Styles = new List<Styles>(){Styles.Funk }
            };

            var result = musicianRepository.GetFiltered(parameter).Result;
            Assert.AreEqual(result.Count, 2);

            parameter.Styles.Add(Styles.Rock);
            result = musicianRepository.GetFiltered(parameter).Result;
            Assert.AreEqual(result.Count, 1);

            parameter.Styles.Add(Styles.HipHop);
            result = musicianRepository.GetFiltered(parameter).Result;
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
