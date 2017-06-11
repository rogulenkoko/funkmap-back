using System.Collections.Generic;
using System.Threading.Tasks;
using Funkmap.Module.Search.Models;

namespace Funkmap.Common.Abstract.Search
{
    public interface ISearchService
    {
        Task<ICollection<SearchModel>> SearchAllAsync();

        Task<ICollection<SearchModel>> SearchNearest(SearchRequest request);
    }
}
