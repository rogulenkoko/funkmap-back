using System.Collections.Generic;
using System.Linq;
using Funkmap.Data.Entities.Entities.Abstract;
using Funkmap.Data.Parameters;
using Funkmap.Data.Services.Abstract;
using MongoDB.Driver;

namespace Funkmap.Data.Services
{
    public class FilterFactory : IFilterFactory
    {
        private readonly ICollection<IFilterService> _filterServices;

        public FilterFactory(ICollection<IFilterService> filterServices)
        {
            _filterServices = filterServices;
        }

        public FilterDefinition<BaseEntity> CreateFilter(IFilterParameter parameter)
        {
            if (parameter == null)
            {
                return Builders<BaseEntity>.Filter.Empty;
            }

            var service = _filterServices.FirstOrDefault(x => x.EntityType == parameter.EntityType);
            if (service == null)
            {
                return Builders<BaseEntity>.Filter.Empty;
            }

            var filter = service.CreateFilter(parameter);
            return filter;
        }
    }
}
