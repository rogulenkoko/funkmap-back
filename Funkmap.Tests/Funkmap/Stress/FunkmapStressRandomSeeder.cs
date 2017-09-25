using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Common;
using Funkmap.Data.Entities;
using Funkmap.Data.Entities.Abstract;
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
            var entitiesCount = 400000;
            var types = Enum.GetValues(typeof(EntityType));
            _random = new Random();

            var tasks = new List<Task>();

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
                        entity = CreateMusician();
                        break;

                    case EntityType.Studio:
                        entity = CreateMusician();
                        break;

                    case EntityType.Shop:
                        entity = CreateMusician();
                        break;

                    case EntityType.RehearsalPoint:
                        entity = CreateMusician();
                        break;
                }

                tasks.Add(_mongoCollection.InsertOneAsync(entity));

            }

            Task.WaitAll(tasks.ToArray());
        }

        private MusicianEntity CreateMusician()
        {
            return new MusicianEntity()
            {
                Address = RandomString(10),
                BandLogin = RandomString(10),
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
                UserLogin = RandomString(15),
                SoundCloudLink = RandomString(15),
                Sex = (Sex)Enum.GetValues(typeof(Sex)).GetValue(_random.Next(Enum.GetValues(typeof(Sex)).Length)),
                Styles = new List<Styles>() { (Styles)Enum.GetValues(typeof(Styles)).GetValue(_random.Next(Enum.GetValues(typeof(Styles)).Length)) }
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
