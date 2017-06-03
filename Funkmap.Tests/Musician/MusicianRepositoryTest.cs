using System.Collections.Generic;
using Funkmap.Musician.Data;
using Funkmap.Musician.Data.Entities;
using Funkmap.Musician.Data.Parameters;
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

            var result = musicianRepository.GetFiltered(parameter);
            Assert.AreEqual(result.Count, 2);

            parameter.Styles.Add(Styles.Rock);
            result = musicianRepository.GetFiltered(parameter);
            Assert.AreEqual(result.Count, 1);

            parameter.Styles.Add(Styles.HipHop);
            result = musicianRepository.GetFiltered(parameter);
            Assert.AreEqual(result.Count, 0);


        }
    }
}
