using System;
using System.Collections.Generic;
using Funkmap.Data;
using Funkmap.Data.Entities.Entities;
using Funkmap.Domain.Enums;
using MongoDB.Driver;
using MongoDB.Driver.GeoJsonObjectModel;

namespace Funkmap.Tests.Data
{
    public class FunkmapDataSeeder
    {
        private readonly IMongoDatabase _database;

        public FunkmapDataSeeder(IMongoDatabase database)
        {
            _database = database;
        }

        public void SeedData()
        {
            SeedMusicians();
            SeedBands();
            SeedShops();
            SeedStudios();
            SeedRehearsalPoints();
        }

        private void SeedMusicians()
        {
            var collection = _database.GetCollection<MusicianEntity>(CollectionNameProvider.BaseCollectionName);

            var musicians = new List<MusicianEntity>()
            {
                new MusicianEntity()
                {
                    Sex = Sex.Male,
                    Login = "rogulenkoko",
                    UserLogin = "test",
                    BirthDate = DateTime.UtcNow,
                    Description = "Описание",
                    Name = "Кирилл Рогуленко",
                    Location = new GeoJsonPoint<GeoJson2DGeographicCoordinates>(new GeoJson2DGeographicCoordinates(30, 50)),
                    Styles = new List<Styles>() { Styles.Funk, Styles.HipHop },
                    Instrument = Instruments.Brass,
                    VkLink = "https://vk.com/id30724049",
                    YouTubeLink = "https://www.youtube.com/user/Urgantshow",
                    BandLogins = new List<string>() { "funkmap", "Kirill'sMother" },
                    ExpirienceType = Expiriences.Advanced,
                    VideoInfos = new List<VideoInfoEntity>() { new VideoInfoEntity() { Id = "mpR5zbEXdW8" }, new VideoInfoEntity() { Id = "GlreDCpb5t0" } },
                    IsActive = true,
                    CreationDate = DateTime.UtcNow.AddMonths(-12),
                    FavoriteFor = new List<string>() { "qwe", "wewe", "dfsdf" }
                },
                new MusicianEntity()
                {
                    Sex = Sex.Female,
                    Login = "madlib",
                    BirthDate = DateTime.UtcNow,
                    UserLogin = "rogulenkoko",
                    Description = "Большое описание музыканта, тудым сюдым. Как дела братва?",
                    Name = "Madlib",
                    Location = new GeoJsonPoint<GeoJson2DGeographicCoordinates>(new GeoJson2DGeographicCoordinates(30, 51)),
                    Styles = new List<Styles>() { Styles.Funk, Styles.Rock },
                    Instrument = Instruments.Drums,
                    FacebookLink = "https://ru-ru.facebook.com/",
                    BandLogins = new List<string>() { "beatles" },
                    ExpirienceType = Expiriences.Beginer,
                    IsActive = true,
                    CreationDate = DateTime.UtcNow.AddMonths(-11),
                    FavoriteFor = new List<string>() { "qwe", "wewe", "dfsdf" }
                },
                new MusicianEntity()
                {
                    Sex = Sex.Male,
                    Login = "razrab",
                    BirthDate = DateTime.UtcNow,
                    UserLogin = "rogulenkoko",
                    Description = "Razrab описание!!!",
                    Name = "Razrab Razrab",
                    Location = new GeoJsonPoint<GeoJson2DGeographicCoordinates>(new GeoJson2DGeographicCoordinates(31, 51)),
                    Styles = new List<Styles>() { Styles.HipHop },
                    Instrument = Instruments.Keyboard,
                    BandLogins = new List<string>() { "metallica" },
                    ExpirienceType = Expiriences.SuperStar,
                    IsActive = true,
                    CreationDate = DateTime.UtcNow.AddMonths(-10),
                    FavoriteFor = new List<string>() { "qwe", "wewe", "dfsdf" }
                },
                new MusicianEntity()
                {
                    Sex = Sex.Male,
                    Login = "norazrab",
                    UserLogin = "rogulenkoko",
                    BirthDate = DateTime.UtcNow,
                    Description = "Razrab описание!!!",
                    Name = "tim tim",
                    Location = new GeoJsonPoint<GeoJson2DGeographicCoordinates>(new GeoJson2DGeographicCoordinates(34, 51)),
                    Styles = new List<Styles>() { Styles.HipHop },
                    Instrument = Instruments.Keyboard,
                    BandLogins = new List<string>(),
                    ExpirienceType = Expiriences.SuperStar,
                    IsActive = true,
                    CreationDate = DateTime.UtcNow.AddMonths(-10),
                    FavoriteFor = new List<string>() { "qwe", "wewe", "dfsdf", "rtyu" }
                }
            };

            collection.InsertManyAsync(musicians).GetAwaiter().GetResult();
        }

        private void SeedBands()
        {
            var collection = _database.GetCollection<BandEntity>(CollectionNameProvider.BaseCollectionName);

            var bands = new List<BandEntity>()
            {
                new BandEntity()
                {
                    UserLogin = "test",
                    DesiredInstruments = new List<Instruments>() { Instruments.Bass, Instruments.Guitar },
                    Name = "The Beatles",
                    Login = "beatles",
                    Location = new GeoJsonPoint<GeoJson2DGeographicCoordinates>(new GeoJson2DGeographicCoordinates(29, 52)),
                    MusicianLogins = new List<string>() { "rogulenkoko", "razrab" },
                    Styles = new List<Styles>() { Styles.Funk, Styles.HipHop },
                    Description = "Даже не буду ничего говорить",
                    VkLink = "vk",
                    IsActive = true,
                    CreationDate = DateTime.UtcNow.AddMonths(-9),
                    FavoriteFor = new List<string>() { "qwe", "wewe" }
                },
                new BandEntity()
                {
                    UserLogin = "rogulenkoko",
                    Name = "Red Hot Chili Peppers",
                    Login = "rhcp",
                    Location = new GeoJsonPoint<GeoJson2DGeographicCoordinates>(new GeoJson2DGeographicCoordinates(28, 52)),
                    MusicianLogins = new List<string>() { "rogulenkoko" },
                    Styles = new List<Styles>() { Styles.Funk, Styles.Rock },
                    Description = "Мы жгучие перцы из солнечной калифорнии и этим все сказано",
                    YouTubeLink = "yt",
                    IsActive = true,
                    CreationDate = DateTime.UtcNow.AddMonths(-8),
                    FavoriteFor = new List<string>() { "qwe", "wewe", "dfsdf" }
                },
                new BandEntity()
                {
                    UserLogin = "rogulenkoko",
                    Name = "Coldplay",
                    Login = "coldplay",
                    Location = new GeoJsonPoint<GeoJson2DGeographicCoordinates>(new GeoJson2DGeographicCoordinates(31, 50)),
                    Styles = new List<Styles>() { Styles.Rock },
                    Description = "Мы грустная группа которая играет холодно",
                    FacebookLink = "asda",
                    VkLink = "aaa",
                    IsActive = true,
                    CreationDate = DateTime.UtcNow.AddMonths(-7),
                    FavoriteFor = new List<string>() { "qwe", "wewe", "dfsdf" }
                }
            };

            collection.InsertManyAsync(bands).GetAwaiter().GetResult();
        }

        private void SeedShops()
        {
            var collection = _database.GetCollection<ShopEntity>(CollectionNameProvider.BaseCollectionName);

            var shops = new List<ShopEntity>()
            {
                new ShopEntity()
                {
                    Login = "guitars",
                    Name = "Гитарушки",
                    Location = new GeoJsonPoint<GeoJson2DGeographicCoordinates>(new GeoJson2DGeographicCoordinates(32, 52)),
                    Website = "https://ru.wikipedia.org/wiki/C_Sharp",
                    Description = "Небольшой магазин с гитарами и прочим барахлом",
                    VkLink = "qwe",
                    Address = "пр-т Независимости 12",
                    IsActive = true,
                    CreationDate = DateTime.UtcNow.AddMonths(-6),
                    FavoriteFor = new List<string>() { "qwe", "wewe", "dfsdf" },
                    UserLogin = "rogulenkoko",
                },
                new ShopEntity()
                {
                    Login = "pinkponk",
                    Name = "Пинк и Понк",
                    Location = new GeoJsonPoint<GeoJson2DGeographicCoordinates>(new GeoJson2DGeographicCoordinates(33, 51)),
                    Website = "http://online-simpsons.ru",
                    VkLink = "qwe",
                    IsActive = true,
                    CreationDate = DateTime.UtcNow.AddMonths(-5),
                    FavoriteFor = new List<string>() { "qwe", "wewe", "dfsdf" },
                    UserLogin = "rogulenkoko",
                },
                new ShopEntity()
                {
                    Login = "monkeyb",
                    Name = "Monkey Business",
                    Location = new GeoJsonPoint<GeoJson2DGeographicCoordinates>(new GeoJson2DGeographicCoordinates(31, 54)),
                    Website = "https://сайт.com",
                    Description = "Большой магазин с гитарами и прочим барахлом",
                    YouTubeLink = "asdasda",
                    Address = "пр-т Каменоостровский 11",
                    IsActive = true,
                    CreationDate = DateTime.UtcNow.AddMonths(-4),
                    FavoriteFor = new List<string>() { "qwe", "wewe", "dfsdf" },
                    UserLogin = "rogulenkoko",
                },
                new ShopEntity()
                {
                    Login = "oneshop",
                    Name = "One-shop",
                    Location = new GeoJsonPoint<GeoJson2DGeographicCoordinates>(new GeoJson2DGeographicCoordinates(31, 51)),
                    Website = "http://tttt.ru",
                    YouTubeLink = "asdasda",
                    FacebookLink = "asdasda",
                    Address = "пр-т Невский 5",
                    IsActive = true,
                    CreationDate = DateTime.UtcNow.AddMonths(-3),
                    FavoriteFor = new List<string>() { "qwe", "wewe", "dfsdf" },
                    UserLogin = "rogulenkoko",
                }
            };

            collection.InsertManyAsync(shops).GetAwaiter().GetResult();
        }

        private void SeedStudios()
        {
            var collection = _database.GetCollection<StudioEntity>(CollectionNameProvider.BaseCollectionName);

            var studios = new List<StudioEntity>()
            {
                new StudioEntity()
                {
                    Login = "blackstar",
                    Name = "Black Star",
                    VkLink = "vk",
                    Location = new GeoJsonPoint<GeoJson2DGeographicCoordinates>(new GeoJson2DGeographicCoordinates(25, 24)),
                    UserLogin = "rogulenkoko",
                    Address = "ул. Мовчанского 12",
                    IsActive = true,
                    CreationDate = DateTime.UtcNow.AddMonths(-2)
                },
                new StudioEntity()
                {
                    Login = "gaz",
                    Name = "Gazgolder",
                    FacebookLink = "face",
                    Location = new GeoJsonPoint<GeoJson2DGeographicCoordinates>(new GeoJson2DGeographicCoordinates(25, 27)),
                    UserLogin = "test",
                    Address = "ул. Торжковская 15",
                    Description = "Супер клевая студия на которой записываются суперклевые пацаны",
                    IsActive = true,
                    CreationDate = DateTime.UtcNow.AddMonths(-1)
                }
            };

            collection.InsertManyAsync(studios).GetAwaiter().GetResult();
        }

        private void SeedRehearsalPoints()
        {
            var collection = _database.GetCollection<RehearsalPointEntity>(CollectionNameProvider.BaseCollectionName);

            var points = new List<RehearsalPointEntity>()
            {
                new RehearsalPointEntity()
                {
                    Login = "monkey",
                    Name = "Monkey Business",
                    Address = "пр-т Мира 12",
                    Location = new GeoJsonPoint<GeoJson2DGeographicCoordinates>(new GeoJson2DGeographicCoordinates(27, 27)),
                    UserLogin = "test",
                    YouTubeLink = "yout",
                    Description = "Точка при магазине, супер оборудование и все такое еу еу",
                    IsActive = true,
                    CreationDate = DateTime.UtcNow
                },
                new RehearsalPointEntity()
                {
                    Login = "grandsound",
                    Name = "Grand Sound",
                    Location = new GeoJsonPoint<GeoJson2DGeographicCoordinates>(new GeoJson2DGeographicCoordinates(29, 27)),
                    UserLogin = "test",
                    YouTubeLink = "yout",
                    IsActive = true,
                    CreationDate = DateTime.UtcNow.AddMonths(-8)
                }
            };

            collection.InsertManyAsync(points).GetAwaiter().GetResult();
        }
    }
}
