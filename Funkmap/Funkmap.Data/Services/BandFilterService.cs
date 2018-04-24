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
    public class BandFilterService : IFilterService
    {
        public EntityType EntityType => EntityType.Band;

        public FilterDefinition<BaseEntity> CreateFilter(IFilterParameter parameter)
        {
            var bandFilterParameter = parameter as BandFilterParameter;

            if (bandFilterParameter == null)
            {
                throw new InvalidOperationException(nameof(parameter));
            }
            
            var filter = Builders<BaseEntity>.Filter.Eq(x => x.EntityType, EntityType.Band);

            if (bandFilterParameter.Styles != null && bandFilterParameter.Styles.Count != 0)
            {
                filter = filter & Builders<BaseEntity>.Filter.All(x => (x as BandEntity).Styles, bandFilterParameter.Styles);
            }
            return filter;
        }
    }
}
