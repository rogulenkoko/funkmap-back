using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Funkmap.Common;
using Funkmap.Common.Data.Mongo;
using Funkmap.Data.Entities;
using Funkmap.Data.Repositories.Abstract;
using MongoDB.Driver;

namespace Funkmap.Data.Repositories
{
    public class MusicianRepository : MongoLoginRepository<MusicianEntity>, IMusicianRepository
    {
        public MusicianRepository(IMongoCollection<MusicianEntity> collection) : base(collection)
        {
        }

        public override Task UpdateAsync(MusicianEntity entity)
        {
            throw new NotImplementedException("Использовать для обновления BaseRepository");
        }

        public override async Task<ICollection<MusicianEntity>> GetAllAsync()
        {
            var filter = Builders<MusicianEntity>.Filter.Eq(x => x.EntityType, EntityType.Musician);
            var result = await _collection.Find(filter).ToListAsync();
            return result;
        }

        public async Task CleanBandDependencies(BandEntity band, string musicianLogin = null)
        {
            if (band?.MusicianLogins == null || band.MusicianLogins.Count == 0) return;

            FilterDefinition<MusicianEntity> musicianFilter;
            UpdateDefinition<MusicianEntity> musicianUpdate = Builders<MusicianEntity>.Update.Pull(x => x.BandLogins, band.Login);

            if (!String.IsNullOrEmpty(musicianLogin))
            {
                musicianFilter = Builders<MusicianEntity>.Filter.Eq(x=>x.Login, musicianLogin) & Builders<MusicianEntity>.Filter.Eq(x=>x.EntityType, EntityType.Musician);
                await _collection.UpdateOneAsync(musicianFilter, musicianUpdate);
            }
            else
            {
                musicianFilter = Builders<MusicianEntity>.Filter.Eq(x => x.EntityType, EntityType.Musician) & Builders<MusicianEntity>.Filter.In(x => x.Login, band.MusicianLogins);
                await _collection.UpdateManyAsync(musicianFilter, musicianUpdate);
            }
           
            
        }
    }
}
