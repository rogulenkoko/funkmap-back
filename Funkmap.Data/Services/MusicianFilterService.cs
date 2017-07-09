using System;
using Funkmap.Common;
using Funkmap.Data.Entities;
using Funkmap.Data.Entities.Abstract;
using Funkmap.Data.Parameters;
using Funkmap.Data.Services.Abstract;
using MongoDB.Driver;

namespace Funkmap.Data.Services
{
    
    public class MusicianFilterService : IFilterService
    {
        public EntityType EntityType => EntityType.Musician;

        public FilterDefinition<BaseEntity> CreateFilter(IFilterParameter parameter)
        {
            if (!(parameter is MusicianFilterParameter))
            {
                throw new InvalidOperationException(nameof(parameter));
            }

            var musicianParameter = parameter as MusicianFilterParameter;

            //db.bases.find({t:1, stls:{$all:[1,3]}, exp:1, intsr:{$in:[1,2]} })
            var filter = Builders<BaseEntity>.Filter.Eq(x => x.EntityType, EntityType.Musician); //todo подумать как для каждого репозитория вынести этот фильтр

            if (musicianParameter.Styles != null && musicianParameter.Styles.Count != 0)
            {
                filter = filter & Builders<BaseEntity>.Filter.All(x => (x as MusicianEntity).Styles, musicianParameter.Styles);
            }

            if (musicianParameter.Expirience != ExpirienceType.None)
            {
                filter = filter & Builders<BaseEntity>.Filter.Eq(x => (x as MusicianEntity).ExpirienceType, musicianParameter.Expirience);
            }

            if (musicianParameter.Instruments != null && musicianParameter.Instruments.Count != 0)
            {
                filter = filter & Builders<BaseEntity>.Filter.In(x => (x as MusicianEntity).Instrument, musicianParameter.Instruments);
            }
            return filter;
        }
    }
}
