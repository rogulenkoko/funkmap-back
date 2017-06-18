using System.Collections.Generic;
using Funkmap.Data.Entities;
using Funkmap.Data.Repositories;
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
            //_musicianRepository = new MusicianRepository(new FakeMusicianDbContext());
        }

        //[TestMethod]
        //public void GetStyleFilteredMusicianTest()
        //{

        //    var parameter = new MusicianParameter()
        //    {
        //        Styles = new List<Styles>() { Styles.Funk }
        //    };

        //    var result = _musicianRepository.GetFiltered(parameter).Result;
        //    Assert.AreEqual(result.Count, 2);

        //    parameter.Styles.Add(Styles.Rock);
        //    result = _musicianRepository.GetFiltered(parameter).Result;
        //    Assert.AreEqual(result.Count, 1);

        //    parameter.Styles.Add(Styles.HipHop);
        //    result = _musicianRepository.GetFiltered(parameter).Result;
        //    Assert.AreEqual(result.Count, 0);


        //}
    }
}
