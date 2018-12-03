using System.Collections.Generic;
using System.Threading.Tasks;
using Funkmap.Domain.Abstract;
using Funkmap.Domain.Abstract.Repositories;
using Funkmap.Domain.Models;
using Funkmap.Domain.Parameters;

namespace Funkmap.Data.Caches.Base
{
    public class BaseQueryCacheRepository : IBaseQueryRepository
    {
        private readonly IBaseQueryRepository _baseQueryRepository;
        private readonly IFavoriteCacheService _favoriteService;

        public BaseQueryCacheRepository(IFavoriteCacheService favoriteCacheService, IBaseQueryRepository baseQueryRepository)
        {
            _baseQueryRepository = baseQueryRepository;
            _favoriteService = favoriteCacheService;

        }

        public Task<Profile> GetAsync(string id)
        {
            return _baseQueryRepository.GetAsync(id);
        }

        public Task<T> GetAsync<T>(string login) where T : Profile
        {
            return _baseQueryRepository.GetAsync<T>(login);
        }

        public Task<List<UserEntitiesCountInfo>> GetUserEntitiesCountInfoAsync(string userLogin)
        {
            return _baseQueryRepository.GetUserEntitiesCountInfoAsync(userLogin);
        }

        public Task<byte[]> GetFileAsync(string login)
        {
            return _baseQueryRepository.GetFileAsync(login);
        }

        public async Task UpdateFavoriteAsync(UpdateFavoriteParameter parameter)
        {
            var favorites = await _baseQueryRepository.GetFavoritesLoginsAsync(parameter.UserLogin);
            await _favoriteService.SetFavorites(parameter.UserLogin, favorites);

        }

        public async Task<List<string>> GetFavoritesLoginsAsync(string userLogin)
        {
            List<string> favorites = await _favoriteService.GetFavoriteLogins(userLogin);
            if (favorites == null)
            {
                favorites = await _baseQueryRepository.GetFavoritesLoginsAsync(userLogin);
                await _favoriteService.SetFavorites(userLogin, favorites);
            }
            return favorites;

        }

        public Task<List<Marker>> GetNearestMarkersAsync(LocationParameter parameter)
        {
            return _baseQueryRepository.GetNearestMarkersAsync(parameter);
        }

        public Task<List<SearchItem>> GetNearestAsync(LocationParameter parameter)
        {
            return _baseQueryRepository.GetNearestAsync(parameter);
        }

        public Task<List<Marker>> GetSpecificMarkersAsync(IReadOnlyCollection<string> logins)
        {
            return _baseQueryRepository.GetSpecificMarkersAsync(logins);
        }

        public Task<List<SearchItem>> GetSpecificAsync(IReadOnlyCollection<string> logins)
        {
            return _baseQueryRepository.GetSpecificAsync(logins);
        }

        public Task<List<string>> GetUserEntitiesLoginsAsync(string userLogin)
        {
            return _baseQueryRepository.GetUserEntitiesLoginsAsync(userLogin);
        }

        public Task<List<Profile>> GetFilteredAsync(CommonFilterParameter commonFilter, IFilterParameter parameter = null)
        {
            return _baseQueryRepository.GetFilteredAsync(commonFilter, parameter);
        }

        public Task<List<Marker>> GetFilteredMarkersAsync(CommonFilterParameter commonFilter, IFilterParameter parameter = null)
        {
            return _baseQueryRepository.GetFilteredMarkersAsync(commonFilter, parameter);
        }

        public Task<long> GetAllFilteredCountAsync(CommonFilterParameter commonFilter, IFilterParameter parameter)
        {
            return _baseQueryRepository.GetAllFilteredCountAsync(commonFilter, parameter);
        }

        public Task<bool> LoginExistsAsync(string login)
        {
            return _baseQueryRepository.LoginExistsAsync(login);
        }
    }
}
