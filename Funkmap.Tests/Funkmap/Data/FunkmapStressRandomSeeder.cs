using System;
using System.Collections.Generic;
using System.Text;
using Funkmap.Data;
using Funkmap.Data.Entities.Entities;
using Funkmap.Data.Entities.Entities.Abstract;
using Funkmap.Domain;
using Funkmap.Domain.Enums;
using MongoDB.Driver;
using MongoDB.Driver.GeoJsonObjectModel;

namespace Funkmap.Tests.Funkmap.Data
{
    public class FunkmapStressRandomSeeder
    {
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<BaseEntity> _mongoCollection;

        private Random _random;
        public FunkmapStressRandomSeeder(IMongoDatabase database)
        {
            _database = database;
            _mongoCollection = _database.GetCollection<BaseEntity>(CollectionNameProvider.BaseCollectionName);
        }

        public void SeedData()
        {
            var entitiesCount = 20000;
            var types = Enum.GetValues(typeof(EntityType));
            _random = new Random();

            for (int i = 0; i < entitiesCount; i++)
            {
                EntityType type = (EntityType)types.GetValue(_random.Next(types.Length));
                BaseEntity entity = null;
                switch (type)
                {
                    case EntityType.Musician:
                        entity = CreateMusician();
                        break;

                    case EntityType.Band:
                        entity = CreateBand();
                        break;

                    case EntityType.Studio:
                        entity = CreateStudio();
                        break;

                    case EntityType.Shop:
                        entity = CreateShop();
                        break;

                    case EntityType.RehearsalPoint:
                        entity = CreateRehearsal();
                        break;
                }

                _mongoCollection.InsertOneAsync(entity).GetAwaiter().GetResult();

            }
        }

        private MusicianEntity CreateMusician()
        {
            return new MusicianEntity()
            {
                Address = RandomString(10),
                BirthDate = DateTime.UtcNow,
                Description = RandomString(40),
                EntityType = EntityType.Musician,
                FacebookLink = RandomString(10),
                YouTubeLink = RandomString(10),
                VkLink = RandomString(10),
                ExpirienceType = (Expiriences)Enum.GetValues(typeof(Expiriences)).GetValue(_random.Next(Enum.GetValues(typeof(Expiriences)).Length)),
                Instrument = (Instruments)Enum.GetValues(typeof(Instruments)).GetValue(_random.Next(Enum.GetValues(typeof(Instruments)).Length)),
                Location = new GeoJsonPoint<GeoJson2DGeographicCoordinates>(new GeoJson2DGeographicCoordinates(_random.Next(0, 90), _random.Next(0, 90))),
                Login = RandomString(10),
                Name = RandomString(15),
                UserLogin = "test",
                SoundCloudLink = RandomString(15),
                Sex = (Sex)Enum.GetValues(typeof(Sex)).GetValue(_random.Next(Enum.GetValues(typeof(Sex)).Length)),
                Styles = new List<Styles>() { (Styles)Enum.GetValues(typeof(Styles)).GetValue(_random.Next(Enum.GetValues(typeof(Styles)).Length)) },
                IsActive = true,
                VideoInfos = new List<VideoInfoEntity>() { new VideoInfoEntity() { Id = "ThCbl10-1pA" } }
            };
        }

        private BandEntity CreateBand()
        {
            return new BandEntity()
            {
                Address = RandomString(10),
                Description = RandomString(40),
                EntityType = EntityType.Band,
                FacebookLink = RandomString(10),
                YouTubeLink = RandomString(10),
                VkLink = RandomString(10),
                Location = new GeoJsonPoint<GeoJson2DGeographicCoordinates>(new GeoJson2DGeographicCoordinates(_random.Next(0, 90), _random.Next(0, 90))),
                Login = RandomString(10),
                Name = RandomString(15),
                UserLogin = "test",
                SoundCloudLink = RandomString(15),
                Styles = new List<Styles>() { (Styles)Enum.GetValues(typeof(Styles)).GetValue(_random.Next(Enum.GetValues(typeof(Styles)).Length)) },
                IsActive = true,
                VideoInfos = new List<VideoInfoEntity>() { new VideoInfoEntity() { Id = "ThCbl10-1pA" } }
            };
        }

        private ShopEntity CreateShop()
        {
            return new ShopEntity()
            {
                Address = RandomString(10),
                Description = RandomString(40),
                EntityType = EntityType.Shop,
                FacebookLink = RandomString(10),
                YouTubeLink = RandomString(10),
                VkLink = RandomString(10),
                Location = new GeoJsonPoint<GeoJson2DGeographicCoordinates>(new GeoJson2DGeographicCoordinates(_random.Next(0, 90), _random.Next(0, 90))),
                Login = RandomString(10),
                Name = RandomString(15),
                UserLogin = "test",
                SoundCloudLink = RandomString(15),
                IsActive = true,
                VideoInfos = new List<VideoInfoEntity>() { new VideoInfoEntity() { Id = "ThCbl10-1pA" } },
                Website = "https://github.com/"
            };
        }

        private StudioEntity CreateStudio()
        {
            return new StudioEntity()
            {
                Address = RandomString(10),
                Description = RandomString(40),
                EntityType = EntityType.Studio,
                FacebookLink = RandomString(10),
                YouTubeLink = RandomString(10),
                VkLink = RandomString(10),
                Location = new GeoJsonPoint<GeoJson2DGeographicCoordinates>(new GeoJson2DGeographicCoordinates(_random.Next(0, 90), _random.Next(0, 90))),
                Login = RandomString(10),
                Name = RandomString(15),
                UserLogin = "test",
                SoundCloudLink = RandomString(15),
                IsActive = true,
                VideoInfos = new List<VideoInfoEntity>() { new VideoInfoEntity() { Id = "ThCbl10-1pA" } }
            };
        }

        private RehearsalPointEntity CreateRehearsal()
        {
            return new RehearsalPointEntity()
            {
                Address = RandomString(10),
                Description = RandomString(40),
                EntityType = EntityType.RehearsalPoint,
                FacebookLink = RandomString(10),
                YouTubeLink = RandomString(10),
                VkLink = RandomString(10),
                Location = new GeoJsonPoint<GeoJson2DGeographicCoordinates>(new GeoJson2DGeographicCoordinates(_random.Next(0, 90), _random.Next(0, 90))),
                Login = RandomString(10),
                Name = RandomString(15),
                UserLogin = "test",
                SoundCloudLink = RandomString(15),
                IsActive = true,
                VideoInfos = new List<VideoInfoEntity>() { new VideoInfoEntity() { Id = "ThCbl10-1pA" } },

            };
        }


        private string RandomString(int Size)
        {
            StringBuilder builder = new StringBuilder();
            char ch;
            for (int i = 0; i < Size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * _random.NextDouble() + 65)));
                builder.Append(ch);
            }
            return builder.ToString();
        }


    }
}
