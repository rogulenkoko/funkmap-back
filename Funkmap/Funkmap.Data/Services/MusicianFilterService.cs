using System;
using Funkmap.Data.Entities.Entities;
using Funkmap.Data.Entities.Entities.Abstract;
using Funkmap.Data.Services.Abstract;
using Funkmap.Domain;
using Funkmap.Domain.Abstract;
using Funkmap.Domain.Parameters;
using MongoDB.Driver;

namespace Funkmap.Data.Services
{
    
    public class MusicianFilterService : IFilterService
    {
        public EntityType EntityType => EntityType.Musician;

        public FilterDefinition<BaseEntity> CreateFilter(IFilterParameter parameter)
        {
            var musicianParameter = parameter as MusicianFilterParameter;

            if (musicianParameter == null)
            {
                throw new InvalidOperationException(nameof(parameter));
            }

            //db.bases.find({t:1, stls:{$all:[1,3]}, exp:1, intsr:{$in:[1,2]} })
            var filter = Builders<BaseEntity>.Filter.Eq(x => x.EntityType, EntityType.Musician); //todo подумать как для каждого репозитория вынести этот фильтр

            if (musicianParameter.Styles != null && musicianParameter.Styles.Count != 0)
            {
                filter = filter & Builders<BaseEntity>.Filter.All(x => (x as MusicianEntity).Styles, musicianParameter.Styles);
            }

            if (musicianParameter.Expiriences != null && musicianParameter.Expiriences.Count != 0)
            {
                filter = filter & Builders<BaseEntity>.Filter.In(x => (x as MusicianEntity).ExpirienceType, musicianParameter.Expiriences);
            }

            if (musicianParameter.Instruments != null && musicianParameter.Instruments.Count != 0)
            {
                filter = filter & Builders<BaseEntity>.Filter.In(x => (x as MusicianEntity).Instrument, musicianParameter.Instruments);
            }
            return filter;
        }
    }
}
