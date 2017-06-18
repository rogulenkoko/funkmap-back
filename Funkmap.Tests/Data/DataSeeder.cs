using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Data.Entities;
using Funkmap.Data.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;
using MongoDB.Driver.GeoJsonObjectModel;

namespace Funkmap.Tests.Data
{
    [TestClass]
    public class DataSeeder
    {
        private readonly IMongoDatabase _database;

        public DataSeeder(IMongoDatabase database)
        {
            _database = database;
        }

        public void SeedData()
        {
            SeedMusicians();
            SeedBands();
        }

        private void SeedMusicians()
        {
            var repository = new MusicianRepository(_database.GetCollection<MusicianEntity>(CollectionNameProvider.BaseCollectionName));

            var m1 = new MusicianEntity()
            {
                Sex = Sex.Male,
                Login = "rogulenkoko",
                BirthDate = DateTime.Now,
                Description = "Описание",
                Name = "Кирилл Рогуленко",
                Location = new GeoJsonPoint<GeoJson2DGeographicCoordinates>(new GeoJson2DGeographicCoordinates(30,50)),
                Styles = new List<Styles>() {Styles.Funk, Styles.HipHop},
                Instrument = InstrumentType.Brass,
                VkLink = "https://vk.com/id30724049",
                YouTubeLink = "https://www.youtube.com/user/Urgantshow",
                BandLogin = "funkmap"
            };


            m1.Photo = ImageProvider.GetAvatar();

            var m2 = new MusicianEntity()
            {
                Sex = Sex.Female,
                Login = "madlib",
                BirthDate = DateTime.Now,
                Description = "Большое описание музыканта, тудым сюдым. Как дела братва?",
                Name = "Madlib",
                Location = new GeoJsonPoint<GeoJson2DGeographicCoordinates>(new GeoJson2DGeographicCoordinates(30, 51)),
                Styles = new List<Styles>() { Styles.Funk, Styles.Rock},
                Instrument = InstrumentType.Drums,
                FacebookLink = "https://ru-ru.facebook.com/",
                BandLogin = "beatles"
            };

            var m3 = new MusicianEntity()
            {
                Sex = Sex.Male,
                Login = "razrab",
                BirthDate = DateTime.Now,
                Description = "Razrab описание!!!",
                Name = "Razrab Razrab",
                Location = new GeoJsonPoint<GeoJson2DGeographicCoordinates>(new GeoJson2DGeographicCoordinates(31, 51)),
                Styles = new List<Styles>() { Styles.HipHop},
                Instrument = InstrumentType.Keyboard,
                BandLogin = "metallica"
            };

            repository.CreateAsync(m1).Wait();
            repository.CreateAsync(m2).Wait();
            repository.CreateAsync(m3).Wait();
        }

        private void SeedBands()
        {
            var repository = new BandRepository(_database.GetCollection<BandEntity>(CollectionNameProvider.BaseCollectionName));

            var b1 = new BandEntity()
            {
                Login = "test",
                DesiredInstruments = new List<InstrumentType>() { InstrumentType.Bass, InstrumentType.Guitar},
                Name = "The Beatles",
                ShowPrice = 123412,
                VideoLinks = new List<string>() { "firstVideo", "secondVideo" },
                Location = new GeoJsonPoint<GeoJson2DGeographicCoordinates>(new GeoJson2DGeographicCoordinates(29, 52))
            };

            var b2 = new BandEntity()
            {
                Login = "rogulenkoko",
                Name = "Red Hot Chili Peppers",
                ShowPrice = 123412,
                VideoLinks = new List<string>() { "firstVideo", "secondVideo" },
                Location = new GeoJsonPoint<GeoJson2DGeographicCoordinates>(new GeoJson2DGeographicCoordinates(28, 52))
            };

            var b3 = new BandEntity()
            {
                Login = "rogulenkoko",
                Name = "Coldplay",
                ShowPrice = 123,
                Location = new GeoJsonPoint<GeoJson2DGeographicCoordinates>(new GeoJson2DGeographicCoordinates(31, 50))
            };

            repository.CreateAsync(b1).Wait();
            repository.CreateAsync(b2).Wait();
            repository.CreateAsync(b3).Wait();
        }
    }
}
