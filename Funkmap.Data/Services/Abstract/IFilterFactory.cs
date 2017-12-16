using Funkmap.Data.Entities.Entities.Abstract;
using Funkmap.Data.Parameters;
using MongoDB.Driver;

namespace Funkmap.Data.Services.Abstract
{
    public interface IFilterFactory
    {
        FilterDefinition<BaseEntity> CreateFilter(IFilterParameter parameter);
    }
}
