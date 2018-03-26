using System.Collections.Generic;
using System.Threading.Tasks;
using Funkmap.Domain.Models;
using Funkmap.Domain.Parameters;

namespace Funkmap.Domain.Abstract.Repositories
{
    public interface IBaseRepository
    {
        Task<Profile> GetAsync(string login);

        Task<T> GetAsync<T>(string login) where T : Profile;

        Task<List<Marker>> GetNearestAsync(LocationParameter parameter);

        Task<List<SearchItem>> GetFullNearestAsync(LocationParameter parameter);

        Task<List<Marker>> GetSpecificNavigationAsync(IReadOnlyCollection<string> logins);

        Task<List<SearchItem>> GetSpecificFullAsync(IReadOnlyCollection<string> logins);

        Task<List<string>> GetUserEntitiesLoginsAsync(string userLogin);

        Task<List<Profile>> GetFilteredAsync(CommonFilterParameter commonFilter, IFilterParameter parameter = null);

        Task<List<Marker>> GetFilteredNavigationAsync(CommonFilterParameter commonFilter, IFilterParameter parameter = null);

        Task<long> GetAllFilteredCountAsync(CommonFilterParameter commonFilter, IFilterParameter parameter);

        Task<bool> CheckIfLoginExistAsync(string login);

        Task<List<UserEntitiesCountInfo>> GetUserEntitiesCountInfoAsync(string userLogin);

        Task<byte[]> GetFileAsync(string fileId);

        Task UpdateFavoriteAsync(UpdateFavoriteParameter parameter);

        Task<List<string>> GetFavoritesLoginsAsync(string userLogin);

        Task UpdateAvatarAsync(string login, byte[] imageBytes);
    }
}
