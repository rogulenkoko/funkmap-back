using System;
using Funkmap.Common;
using Funkmap.Data.Entities;
using Funkmap.Data.Entities.Abstract;
using Funkmap.Data.Parameters;
using Funkmap.Data.Services.Abstract;
using MongoDB.Driver;

namespace Funkmap.Data.Services
{
    
    public class BandFilterService : IFilterService
    {
        public EntityType EntityType => EntityType.Band;

        public FilterDefinition<BaseEntity> CreateFilter(IFilterParameter parameter)
        {
            if (!(parameter is BandFilterParameter))
            {
                throw new InvalidOperationException(nameof(parameter));
            }

            var bandFilterParameter = parameter as BandFilterParameter;
            
            var filter = Builders<BaseEntity>.Filter.Eq(x => x.EntityType, EntityType.Band);

            if (bandFilterParameter.Styles != null && bandFilterParameter.Styles.Count != 0)
            {
                filter = filter & Builders<BaseEntity>.Filter.All(x => (x as BandEntity).Styles, bandFilterParameter.Styles);
            }
            return filter;
        }
    }
}
