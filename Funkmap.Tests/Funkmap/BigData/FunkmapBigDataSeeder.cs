using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Data.Entities;
using Funkmap.Data.Entities.Abstract;
using Funkmap.Data.Repositories;
using Funkmap.Data.Repositories.Abstract;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;
using MongoDB.Driver.GeoJsonObjectModel;

namespace Funkmap.Tests.Funkmap.BigData
{
    [TestClass]
    class FunkmapBigDataSeeder
    {
        private readonly IMongoDatabase _database;
        private readonly List<string> _name;
        private readonly List<string> _surname;

        public FunkmapBigDataSeeder(IMongoDatabase database)
        {
            _database = database;
            _name = new List<string>();
            _surname = new List<string>();
            Initialize();
        }

        public void SeedData()
        {
            SeedMusicians();
            SeedBands();
            SeedShops();
            SeedStudios();
            SeedRehearsalPoints();
        }

        private void Initialize()
        {
            DirectoryInfo dir = Directory.GetParent(".");
            dir = dir.Parent;
            string path = dir.FullName+"\\names";
            string line;
            StreamReader sr = new StreamReader(path + "\\name.txt");
            while (!sr.EndOfStream)
            {
                line = sr.ReadLine();
                _name.Add(line);
            }
            sr = new StreamReader(path + "\\subname.txt");
            while (!sr.EndOfStream)
            {
                line = sr.ReadLine();
                if(line=="")
                    continue;
              _surname.Add(line);
            }
        }

        private void SeedMusicians()
        {
            var repository =
                new MusicianRepository(
                    _database.GetCollection<MusicianEntity>(CollectionNameProvider.BaseCollectionName));
            for (int i = 0; i < _surname.Count; i++)
            {
                
                for (int j = 0; j < _name.Count; j++)
                {
                
                    var musician = new MusicianEntity()
                    {
                        Sex = Sex.Male,
                        Login = _name[j].Substring(0, 1) + _surname[i],
                        UserLogin = _name[j] + (i*_name.Count+j).ToString(),
                        BirthDate = DateTime.Now,
                        Description = "Описание",
                        Name = _name[j] + " " + _surname[i],
                        Location = new GeoJsonPoint<GeoJson2DGeographicCoordinates>(new GeoJson2DGeographicCoordinates(
                            (double) new Random().Next(25000, 35000) / 1000,
                            (double) new Random().Next(45000, 55000) / 1000)),
                        Styles =
                            new List<Styles>() {(Styles) new Random().Next(0, 4), (Styles) new Random().Next(0, 4)},
                        Instrument = (InstrumentType) new Random().Next(0, 6),
                        VkLink = "https://vk.com/" + _name[j].Substring(0, 1) + _surname[i],
                        // YouTubeLink = "https://www.youtube.com/user/Urgantshow",
                        // BandLogin = "funkmap",
                        ExpirienceType = (ExpirienceType) new Random().Next(0, 4),
                        //VideoInfos = new List<VideoInfo>() { new VideoInfo() { Id = "mpR5zbEXdW8" }, new VideoInfo() { Id = "GlreDCpb5t0" } }
                    };
                    repository.CreateAsync(musician).Wait();
                }
            }
        }

        private void SeedBands()
        {
            var repository =
                new BandRepository(_database.GetCollection<BandEntity>(CollectionNameProvider.BaseCollectionName));
            for (int i = 0; i < _name.Count-1; i++)
            {
                if(i==_surname.Count)
                    break;
                var b1 = new BandEntity()
                {
                    UserLogin = "testBandEntity" + i,
                    DesiredInstruments = new List<InstrumentType>()
                    {
                        (InstrumentType) new Random().Next(0, 6),
                        (InstrumentType) new Random().Next(0, 6)
                    },
                    Name = "testName" + i,
                    Login = "testLogin" + i,
                    VideoLinks = new List<string>() {"firstVideo", "secondVideo"},
                    Location = new GeoJsonPoint<GeoJson2DGeographicCoordinates>(new GeoJson2DGeographicCoordinates(
                        (double) new Random().Next(25000, 35000) / 1000,
                        (double) new Random().Next(45000, 55000) / 1000)),
                    MusicianLogins = new List<string>()
                    {
                        _name[i].Substring(0, 1) + _surname[i],
                        _name[i + 1].Substring(0, 1) + _surname[i]
                    },
                    Styles = new List<Styles>() {(Styles) new Random().Next(0, 4), (Styles) new Random().Next(0, 4)},
                    Description = "Даже не буду ничего говорить",
                    VkLink = "vk"
                };
                repository.CreateAsync(b1).Wait();
            }
        }

        private void SeedShops()
        {
            var repository =
                new ShopRepository(_database.GetCollection<ShopEntity>(CollectionNameProvider.BaseCollectionName));
            for (int i = 0; i < 200; i++)
            {
                var s1 = new ShopEntity()
                {
                    Login = "IShop" + i,
                    Name = "магазик" + i,
                    Location = new GeoJsonPoint<GeoJson2DGeographicCoordinates>(new GeoJson2DGeographicCoordinates(
                        (double) new Random().Next(25000, 35000) / 1000,
                        (double) new Random().Next(45000, 55000) / 1000)),
                    Website = "https://ru.wikipedia.org/wiki/C_Sharp" + i,
                    Description = "Небольшой магазин с гитарами и прочим барахлом",
                    VkLink = "qwe" + i,
                    Address = "пр-т Независимости " + i
                };
                repository.CreateAsync(s1).Wait();
            }
        }

        private void SeedStudios()
        {
            var repository =
                new StudioRepository(_database.GetCollection<StudioEntity>(CollectionNameProvider.BaseCollectionName));
            for (int i = 0; i < _surname.Count-2; i++)
            {
                if(i==_name.Count)
                    break;
                var s1 = new StudioEntity()
                {
                    Login = "IStudio" + i,
                    Name = "INameStudio" + i,
                    VkLink = "vk" + i,
                    Location = new GeoJsonPoint<GeoJson2DGeographicCoordinates>(new GeoJson2DGeographicCoordinates(
                        (double) new Random().Next(25000, 35000) / 1000,
                        (double) new Random().Next(45000, 55000) / 1000)),
                    UserLogin = _name[i].Substring(0, 1) + _surname[i + 2],
                    Address = "ул. Мовчанского " + i
                };
                repository.CreateAsync(s1).Wait();
            }
        }
        private void SeedRehearsalPoints()
        {
            var repository = new RehearsalPointRepository(_database.GetCollection<RehearsalPointEntity>(CollectionNameProvider.BaseCollectionName));
            for(int i = 0; i < 200; i++)
            {
                var r1 = new RehearsalPointEntity()
                {
                    Login = "monkey"+i,
                    Name = "Monkey Business"+i,
                    Address = "пр-т Мира "+i,
                    Location = new GeoJsonPoint<GeoJson2DGeographicCoordinates>(new GeoJson2DGeographicCoordinates(
                        (double)new Random().Next(25000, 35000) / 1000,
                        (double)new Random().Next(45000, 55000) / 1000)),
                    UserLogin = "testRehearsalPoint" + i,
                    YouTubeLink = "yout",
                    Description = "Точка при магазине, супер оборудование и все такое еу еу"
                };

                repository.CreateAsync(r1).Wait();
            }
            
        }
    }
}