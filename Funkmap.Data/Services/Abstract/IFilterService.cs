using Funkmap.Data.Entities.Entities.Abstract;
using Funkmap.Domain;
using Funkmap.Domain.Abstract;
using MongoDB.Driver;

namespace Funkmap.Data.Services.Abstract
{
    public interface IFilterService
    {
        EntityType EntityType { get; }
        FilterDefinition<BaseEntity> CreateFilter(IFilterParameter parameter);
    }
}
