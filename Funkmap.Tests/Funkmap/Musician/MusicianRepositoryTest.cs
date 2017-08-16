using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Funkmap.Data.Entities;
using Funkmap.Data.Parameters;
using Funkmap.Data.Repositories;
using Funkmap.Data.Repositories.Abstract;
using Funkmap.Tests.Funkmap.Data;
using Funkmap.Tools;
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
            _musicianRepository = new MusicianRepository(FunkmapTestDbProvider.DropAndCreateDatabase.GetCollection<MusicianEntity>(CollectionNameProvider.BaseCollectionName));
        }

        [TestMethod]
        public void UpdateMusicianTest()
        {
            var musician = new MusicianEntity()
            {
                Login = "zzzz",
                Name = "Кирилл Рогуленко"
            };

            _musicianRepository.CreateAsync(musician).GetAwaiter().GetResult();

            MusicianEntity createdMusician = _musicianRepository.GetAsync(musician.Login).GetAwaiter().GetResult();

            createdMusician.Name = "Рогуленко Кирилл";
            createdMusician.Instrument = InstrumentType.Drums;
            createdMusician.BirthDate = DateTime.Now;

            _musicianRepository.UpdateAsync(createdMusician).GetAwaiter().GetResult();


            MusicianEntity updatedMusician = _musicianRepository.GetAsync(musician.Login).GetAwaiter().GetResult();

            Assert.AreEqual(updatedMusician.Login, musician.Login);
            Assert.AreNotEqual(updatedMusician.Instrument, musician.Instrument);
            Assert.AreNotEqual(updatedMusician.Name, musician.Name);
        }


        [TestMethod]
        public void UpdateWudthExtension()
        {
            
            var update = new MusicianEntity()
            {
                Sex = Sex.Female,
                Instrument = InstrumentType.Bass
            };

            var existingMusician = _musicianRepository.GetAllAsync().GetAwaiter().GetResult().First();

            existingMusician.FillEntity<MusicianEntity>(update);

            _musicianRepository.UpdateAsync(existingMusician).Wait();

            var updatedMusician = _musicianRepository.GetAsync(existingMusician.Login).GetAwaiter().GetResult();
            
        }


        [TestMethod]
        public void UpdateEntityExtensionTest()
        {
            var entity = new MusicianEntity()
            {
                Login = "asd",
                Description = "aaaaa"
            };

            var newEntity = new MusicianEntity()
            {
                Login = "asd",
                BirthDate = DateTime.Now,
                Description = "zzzzz"
            };

            entity.FillEntity<MusicianEntity>(newEntity);

            Assert.AreEqual(entity.Login, newEntity.Login);

            Assert.AreEqual(entity.BirthDate, newEntity.BirthDate);
            Assert.AreEqual(entity.Description, newEntity.Description);

        }
    }
}
