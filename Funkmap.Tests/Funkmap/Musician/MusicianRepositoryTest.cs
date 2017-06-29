using System.Collections.Generic;
using Funkmap.Data.Entities;
using Funkmap.Data.Entities.Abstract;
using Funkmap.Data.Parameters;
using Funkmap.Data.Repositories;
using Funkmap.Data.Repositories.Abstract;
using Funkmap.Tests.Funkmap.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Funkmap.Tests.Funkmap.Musician
{
    [TestClass]
    public class MusicianRepositoryTest
    {
        private IMusicianRepository _musicianRepository;

        [TestInitialize]
        public void Initialize()
        {
            _musicianRepository = new MusicianRepository(FunkmapDbProvider.DropAndCreateDatabase.GetCollection<MusicianEntity>(CollectionNameProvider.BaseCollectionName));
        }

        [TestMethod]
        public void GetStyleFilteredMusicianTest()
        {
            var parameter = new MusicianFilterParameter()
            {
                Styles = new List<Styles>() { Styles.Funk }
            };

            var result = _musicianRepository.GetFilteredMusicians(parameter).Result;
            Assert.AreEqual(result.Count, 2);

            parameter.Styles.Add(Styles.Rock);
            result = _musicianRepository.GetFilteredMusicians(parameter).Result;
            Assert.AreEqual(result.Count, 1);

            parameter.Styles.Add(Styles.HipHop);
            result = _musicianRepository.GetFilteredMusicians(parameter).Result;
            Assert.AreEqual(result.Count, 0);


        }
    }
}
