using System.Collections.Generic;
using System.Threading.Tasks;

namespace Funkmap.Common.Abstract.Search
{
    public interface ISearchService
    {
        Task<ICollection<SearchModel>> SearchAllAsync();
    }
}
