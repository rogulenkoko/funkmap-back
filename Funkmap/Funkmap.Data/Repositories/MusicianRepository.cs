using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Funkmap.Data.Entities.Entities;
using Funkmap.Domain;
using Funkmap.Domain.Abstract.Repositories;
using Funkmap.Domain.Models;
using MongoDB.Driver;

namespace Funkmap.Data.Repositories
{
    public class MusicianRepository : IMusicianRepository
    {
        private readonly IMongoCollection<MusicianEntity> _collection;
        
        public MusicianRepository(IMongoCollection<MusicianEntity> collection)
        {
            _collection = collection;
        }

        public async Task ProcessBandDependenciesAsync(Band band, Band updatedBand = null)
        {
            if (band.Musicians == null)
            {
                band.Musicians = new List<string>();
            }

            List<string> updatedBandMusicianLogins;

            if (updatedBand?.Musicians == null)
            {
                updatedBandMusicianLogins = new List<string>();
            }
            else
            {
                updatedBandMusicianLogins = updatedBand.Musicians;
            }

            var addedMusicianLogins = updatedBandMusicianLogins.Except(band.Musicians).ToList();
            var deletedMusicianLogins = band.Musicians.Except(updatedBandMusicianLogins).ToList();

            if (addedMusicianLogins.Any())
            {
                var musicianUpdate = Builders<MusicianEntity>.Update.Push(x => x.BandLogins, band.Login);

                var musicianFilter = Builders<MusicianEntity>.Filter.Eq(x => x.EntityType, EntityType.Musician) 
                                     & Builders<MusicianEntity>.Filter.In(x => x.Login, addedMusicianLogins)
                                     & Builders<MusicianEntity>.Filter.Where(x => !x.BandLogins.Contains(band.Login));

                await _collection.UpdateManyAsync(musicianFilter, musicianUpdate);
            }

            if (deletedMusicianLogins.Any())
            {
                var musicianUpdate = Builders<MusicianEntity>.Update.Pull(x => x.BandLogins, band.Login);

                var musicianFilter = Builders<MusicianEntity>.Filter.Eq(x => x.EntityType, EntityType.Musician) 
                                     & Builders<MusicianEntity>.Filter.In(x => x.Login, deletedMusicianLogins)
                                     & Builders<MusicianEntity>.Filter.AnyEq(x => x.BandLogins, band.Login);

                await _collection.UpdateManyAsync(musicianFilter, musicianUpdate);
            }
        }
    }
}
