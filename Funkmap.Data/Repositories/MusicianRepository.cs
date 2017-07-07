using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Funkmap.Common;
using Funkmap.Common.Data.Mongo;
using Funkmap.Data.Entities;
using Funkmap.Data.Parameters;
using Funkmap.Data.Repositories.Abstract;
using MongoDB.Driver;

namespace Funkmap.Data.Repositories
{
    public class MusicianRepository : MongoLoginRepository<MusicianEntity>, IMusicianRepository
    {
        private readonly IMongoCollection<MusicianEntity> _collection;

        public MusicianRepository(IMongoCollection<MusicianEntity> collection) : base(collection)
        {
            _collection = collection;
        }

        public override Task<UpdateResult> UpdateAsync(MusicianEntity entity)
        {
            throw new NotImplementedException();
        }

        public override async Task<ICollection<MusicianEntity>> GetAllAsync()
        {
            var filter = Builders<MusicianEntity>.Filter.Eq(x => x.EntityType, EntityType.Musician);
            var result = await _collection.Find(filter).ToListAsync();
            return result;
        }

        public async Task<ICollection<MusicianEntity>> GetFilteredMusiciansAsync(MusicianFilterParameter parameter)
        {
            //db.bases.find({t:1, stls:{$all:[1,3]}, exp:1, intsr:{$in:[1,2]} })
            var filter = Builders<MusicianEntity>.Filter.Eq(x => x.EntityType, EntityType.Musician); //todo подумать как для каждого репозитория вынести этот фильтр

            if (parameter.Styles != null && parameter.Styles.Count != 0)
            {
                filter = filter & Builders<MusicianEntity>.Filter.All(x => x.Styles, parameter.Styles);
            }

            if (parameter.Expirience != ExpirienceType.None)
            {
                filter = filter & Builders<MusicianEntity>.Filter.Eq(x => x.ExpirienceType, parameter.Expirience);
            }

            if (parameter.Instruments != null && parameter.Instruments.Count != 0)
            {
                filter = filter & Builders<MusicianEntity>.Filter.In(x => x.Instrument, parameter.Instruments);
            }
            var result = await _collection.Find(filter).ToListAsync();
            return result;
        }
    }
}
