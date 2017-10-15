using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Common;
using Funkmap.Data.Entities;
using Funkmap.Data.Entities.Abstract;
using Funkmap.Tests.Images;
using MongoDB.Driver;
using MongoDB.Driver.GeoJsonObjectModel;

namespace Funkmap.Tests.Funkmap.Stress
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
                ExpirienceType = (ExpirienceType)Enum.GetValues(typeof(ExpirienceType)).GetValue(_random.Next(Enum.GetValues(typeof(ExpirienceType)).Length)),
                Instrument = (InstrumentType)Enum.GetValues(typeof(InstrumentType)).GetValue(_random.Next(Enum.GetValues(typeof(InstrumentType)).Length)),
                Location = new GeoJsonPoint<GeoJson2DGeographicCoordinates>(new GeoJson2DGeographicCoordinates(_random.Next(0, 90), _random.Next(0, 90))),
                Login = RandomString(10),
                Name = RandomString(15),
                UserLogin = "test",
                SoundCloudLink = RandomString(15),
                Sex = (Sex)Enum.GetValues(typeof(Sex)).GetValue(_random.Next(Enum.GetValues(typeof(Sex)).Length)),
                Styles = new List<Styles>() { (Styles)Enum.GetValues(typeof(Styles)).GetValue(_random.Next(Enum.GetValues(typeof(Styles)).Length)) },
                Photo = new ImageInfo() { Image = ImageProvider.GetAvatar("avatar.jpg") },
                IsActive = true,
                VideoInfos = new List<VideoInfo>() { new VideoInfo() { Id = "ThCbl10-1pA" } }
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
                Photo = new ImageInfo() { Image = ImageProvider.GetAvatar("avatar.jpg") },
                IsActive = true,
                VideoInfos = new List<VideoInfo>() { new VideoInfo() { Id = "ThCbl10-1pA" } }
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
                Photo = new ImageInfo() { Image = ImageProvider.GetAvatar("avatar.jpg") },
                IsActive = true,
                VideoInfos = new List<VideoInfo>() { new VideoInfo() { Id = "ThCbl10-1pA" } },
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
                Photo = new ImageInfo() { Image = ImageProvider.GetAvatar("avatar.jpg") },
                IsActive = true,
                VideoInfos = new List<VideoInfo>() { new VideoInfo() { Id = "ThCbl10-1pA" } }
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
                Photo = new ImageInfo() { Image = ImageProvider.GetAvatar("avatar.jpg") },
                IsActive = true,
                VideoInfos = new List<VideoInfo>() { new VideoInfo() { Id = "ThCbl10-1pA" } },

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
