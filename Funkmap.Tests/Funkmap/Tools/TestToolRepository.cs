using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Funkmap.Data.Entities.Entities;
using Funkmap.Data.Entities.Entities.Abstract;
using Funkmap.Data.Mappers;
using Funkmap.Domain;
using Funkmap.Domain.Abstract;
using Funkmap.Domain.Models;
using MongoDB.Driver;

namespace Funkmap.Tests.Funkmap.Tools
{
    internal class TestToolRepository
    {
        private readonly IMongoCollection<BaseEntity> _collection;
        
        public IMongoCollection<MusicianEntity> MusicianCollection { get; set; }
        public IMongoCollection<BandEntity> BandCollection { get; set; }

        public TestToolRepository(IMongoCollection<BaseEntity> collection)
        {
            _collection = collection;
        }

        public async Task<List<string>> GetAnyLoginsAsync(long? count = null)
        {
            var filter = Builders<BaseEntity>.Filter.Empty;

            List<BaseEntity> some;

            if (!count.HasValue)
            {
                some = await _collection.Find(filter).ToListAsync();
            }
            else
            {
                some = await _collection.Find(filter).Limit((int)count.Value).ToListAsync();
            }

            return some.Select(x => x.Login).ToList();
        }

        public DistanceResult GetDistances(ILocationParameter parameter)
        {
            var longitude = parameter.Longitude.Value.ToString(CultureInfo.InvariantCulture);
            var latitude = parameter.Latitude.Value.ToString(CultureInfo.InvariantCulture);
            var command = new JsonCommand<DistanceResult>($"{{ geoNear: 'bases',near: [ { longitude }, { latitude } ], spherical: true}}");
            return _collection.Database.RunCommand(command);
        }

        public async Task<List<string>> GetProfileUsersAsync()
        {
            var projection = Builders<BaseEntity>.Projection.Include(x => x.UserLogin);
            var entities = await _collection.Find(Builders<BaseEntity>.Filter.Empty).Project<BaseEntity>(projection).ToListAsync();
            return entities.Select(x => x.UserLogin).ToList();
        }

        public async Task<List<Musician>> GetBandRelatedMusiciansAsync(string bandLogin, int count, bool isParticipant)
        {
            FilterDefinition<MusicianEntity> filter = Builders<MusicianEntity>.Filter.Eq(x => x.EntityType, EntityType.Musician);

            if (isParticipant)
            {
                filter = filter & Builders<MusicianEntity>.Filter.AnyEq(x => x.BandLogins, bandLogin);
            }
            else
            {
                filter = filter & Builders<MusicianEntity>.Filter.Where(x => !x.BandLogins.Contains(bandLogin));
            }

            var entities = await MusicianCollection.Find(filter).Limit(count).ToListAsync();

            return entities.ToSpecificProfiles().Select(x => x as Musician).ToList();
        }

        public async Task<List<Band>> GetMusicianRelatedBandsAsync(string musicianLogin, int count, bool isParticipant)
        {
            FilterDefinition<BandEntity> filter = Builders<BandEntity>.Filter.Eq(x => x.EntityType, EntityType.Band);

            if (isParticipant)
            {
                filter = filter & Builders<BandEntity>.Filter.AnyEq(x => x.MusicianLogins, musicianLogin);
            }
            else
            {
                filter = filter & Builders<BandEntity>.Filter.Where(x => !x.MusicianLogins.Contains(musicianLogin));
            }

            var entities = await BandCollection.Find(filter).Limit(count).ToListAsync();

            return entities.ToSpecificProfiles().Select(x => x as Band).ToList();
        }
    }
}
