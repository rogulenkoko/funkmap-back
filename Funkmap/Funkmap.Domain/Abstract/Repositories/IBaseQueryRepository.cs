using System.Collections.Generic;
using System.Threading.Tasks;
using Funkmap.Domain.Models;
using Funkmap.Domain.Parameters;

namespace Funkmap.Domain.Abstract.Repositories
{
    public interface IBaseQueryRepository
    {
        Task<Profile> GetAsync(string login);

        Task<T> GetAsync<T>(string login) where T : Profile;

        Task<List<Marker>> GetNearestMarkersAsync(LocationParameter parameter);

        Task<List<SearchItem>> GetNearestAsync(LocationParameter parameter);

        Task<List<Marker>> GetSpecificMarkersAsync(IReadOnlyCollection<string> logins);

        Task<List<SearchItem>> GetSpecificAsync(IReadOnlyCollection<string> logins);

        Task<List<string>> GetUserEntitiesLoginsAsync(string userLogin);

        Task<List<Profile>> GetFilteredAsync(CommonFilterParameter commonFilter, IFilterParameter parameter = null);

        Task<List<Marker>> GetFilteredMarkersAsync(CommonFilterParameter commonFilter, IFilterParameter parameter = null);

        Task<long> GetAllFilteredCountAsync(CommonFilterParameter commonFilter, IFilterParameter parameter = null);

        Task<bool> LoginExistsAsync(string login);

        Task<List<UserEntitiesCountInfo>> GetUserEntitiesCountInfoAsync(string userLogin);

        Task<byte[]> GetFileAsync(string login);
        
        Task<List<string>> GetFavoritesLoginsAsync(string userLogin);
        
    }
}
