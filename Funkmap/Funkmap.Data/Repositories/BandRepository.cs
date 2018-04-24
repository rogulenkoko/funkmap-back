using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Funkmap.Common.Data.Mongo;
using Funkmap.Data.Entities.Entities;
using Funkmap.Domain;
using Funkmap.Domain.Abstract.Repositories;
using Funkmap.Domain.Models;
using MongoDB.Driver;

namespace Funkmap.Data.Repositories
{
    public class BandRepository : RepositoryBase<BandEntity>, IBandRepository
    {
        public BandRepository(IMongoCollection<BandEntity> collection) : base(collection)
        {
        }

        public async Task ProcessMusicianDependenciesAsync(Musician musician, Musician updatedMusician = null)
        {
            if (musician.BandLogins == null)
            {
                musician.BandLogins = new List<string>();
            }

            List<string> updatedMusicianBandLogins;

            if (updatedMusician?.BandLogins == null)
            {
                updatedMusicianBandLogins = new List<string>();
            }
            else
            {
                updatedMusicianBandLogins = updatedMusician.BandLogins;
            }

            var addedBandlogins = updatedMusicianBandLogins.Except(musician.BandLogins).ToList();
            var deletedBandLogins = musician.BandLogins.Except(updatedMusicianBandLogins).ToList();

            if (addedBandlogins.Any())
            {
                var musicianUpdate = Builders<BandEntity>.Update.Push(x => x.MusicianLogins, musician.Login);

                var musicianFilter = Builders<BandEntity>.Filter.Eq(x => x.EntityType, EntityType.Band) & Builders<BandEntity>.Filter.In(x => x.Login, addedBandlogins);
                await _collection.UpdateManyAsync(musicianFilter, musicianUpdate);
            }

            if (deletedBandLogins.Any())
            {
                var musicianUpdate = Builders<BandEntity>.Update.Pull(x => x.MusicianLogins, musician.Login);

                var musicianFilter = Builders<BandEntity>.Filter.Eq(x => x.EntityType, EntityType.Band) & Builders<BandEntity>.Filter.In(x => x.Login, deletedBandLogins);
                await _collection.UpdateManyAsync(musicianFilter, musicianUpdate);
            }
        }
    }
}
