using System;
using System.Threading.Tasks;
using Funkmap.Common.Data.Mongo;
using Funkmap.Data.Entities.Entities;
using Funkmap.Domain;
using Funkmap.Domain.Abstract.Repositories;
using Funkmap.Domain.Models;
using MongoDB.Driver;

namespace Funkmap.Data.Repositories
{
    public class MusicianRepository : LoginRepository<MusicianEntity>, IMusicianRepository
    {
        public MusicianRepository(IMongoCollection<MusicianEntity> collection) : base(collection)
        {
        }

        public override Task UpdateAsync(MusicianEntity entity)
        {
            throw new NotImplementedException("Использовать для обновления BaseRepository");
        }

        public async Task CleanBandDependencies(Band band, string musicianLogin = null)
        {
            if (band?.Musicians == null || band.Musicians.Count == 0) return;

            FilterDefinition<MusicianEntity> musicianFilter;
            UpdateDefinition<MusicianEntity> musicianUpdate = Builders<MusicianEntity>.Update.Pull(x => x.BandLogins, band.Login);

            if (!String.IsNullOrEmpty(musicianLogin))
            {
                musicianFilter = Builders<MusicianEntity>.Filter.Eq(x=>x.Login, musicianLogin) & Builders<MusicianEntity>.Filter.Eq(x=>x.EntityType, EntityType.Musician);
                await _collection.UpdateOneAsync(musicianFilter, musicianUpdate);
            }
            else
            {
                musicianFilter = Builders<MusicianEntity>.Filter.Eq(x => x.EntityType, EntityType.Musician) & Builders<MusicianEntity>.Filter.In(x => x.Login, band.Musicians);
                await _collection.UpdateManyAsync(musicianFilter, musicianUpdate);
            }
        }
    }
}
