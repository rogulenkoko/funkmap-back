using Funkmap.Data.Entities;
using Funkmap.Data.Entities.Entities.Abstract;
using Funkmap.Data.Parameters;
using MongoDB.Driver;

namespace Funkmap.Data.Services.Abstract
{
    public interface IFilterService
    {
        EntityType EntityType { get; }
        FilterDefinition<BaseEntity> CreateFilter(IFilterParameter parameter);
    }
}
