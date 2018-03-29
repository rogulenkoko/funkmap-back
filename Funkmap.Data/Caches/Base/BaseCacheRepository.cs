using System.Collections.Generic;
using System.Threading.Tasks;
using Funkmap.Data.Entities.Entities.Abstract;
using Funkmap.Domain.Abstract;
using Funkmap.Domain.Abstract.Repositories;
using Funkmap.Domain.Models;
using Funkmap.Domain.Parameters;

namespace Funkmap.Data.Caches.Base
{
    public class BaseCacheRepository : IBaseRepository
    {
        private readonly IBaseRepository _baseRepository;
        private readonly IFavoriteCacheService _favoriteService;

        public BaseCacheRepository(IFavoriteCacheService favoriteCacheService, IBaseRepository baseRepository)
        {
            _baseRepository = baseRepository;
            _favoriteService = favoriteCacheService;

        }

        #region UpdateAsync

        public Task<Profile> DeleteAsync(string id)
        {
            //todo решить как быть здесь с избранными
            return _baseRepository.DeleteAsync(id);
        }

        #endregion

        public Task<Profile> GetAsync(string id)
        {
            return _baseRepository.GetAsync(id);
        }

        public Task<T> GetAsync<T>(string login) where T : Profile
        {
            return _baseRepository.GetAsync<T>(login);
        }

        public Task<List<UserEntitiesCountInfo>> GetUserEntitiesCountInfoAsync(string userLogin)
        {
            return _baseRepository.GetUserEntitiesCountInfoAsync(userLogin);
        }

        public Task<byte[]> GetFileAsync(string fileId)
        {
            return _baseRepository.GetFileAsync(fileId);
        }

        public async Task UpdateFavoriteAsync(UpdateFavoriteParameter parameter)
        {
            
            await _baseRepository.UpdateFavoriteAsync(parameter);

            var favorites = await _baseRepository.GetFavoritesLoginsAsync(parameter.UserLogin);
            _favoriteService.SetFavorites(parameter.UserLogin, favorites);

        }

        public async Task<List<string>> GetFavoritesLoginsAsync(string userLogin)
        {
            List<string> favorites = await _favoriteService.GetFavoriteLogins(userLogin);
            if (favorites == null)
            {
                favorites = await _baseRepository.GetFavoritesLoginsAsync(userLogin);
                _favoriteService.SetFavorites(userLogin, favorites);
            }
            return favorites;

        }

        public Task UpdateAvatarAsync(string login, byte[] imageBytes)
        {
            return _baseRepository.UpdateAvatarAsync(login, imageBytes);
        }

        public Task<List<Marker>> GetNearestMarkersAsync(LocationParameter parameter)
        {
            return _baseRepository.GetNearestMarkersAsync(parameter);
        }

        public Task<List<SearchItem>> GetNearestAsync(LocationParameter parameter)
        {
            return _baseRepository.GetNearestAsync(parameter);
        }

        public Task<List<Marker>> GetSpecificMarkersAsync(IReadOnlyCollection<string> logins)
        {
            return _baseRepository.GetSpecificMarkersAsync(logins);
        }

        public Task<List<SearchItem>> GetSpecificAsync(IReadOnlyCollection<string> logins)
        {
            return _baseRepository.GetSpecificAsync(logins);
        }

        public Task<List<string>> GetUserEntitiesLoginsAsync(string userLogin)
        {
            return _baseRepository.GetUserEntitiesLoginsAsync(userLogin);
        }

        public Task<List<Profile>> GetFilteredAsync(CommonFilterParameter commonFilter, IFilterParameter parameter = null)
        {
            return _baseRepository.GetFilteredAsync(commonFilter, parameter);
        }

        public Task<List<Marker>> GetFilteredMarkersAsync(CommonFilterParameter commonFilter, IFilterParameter parameter = null)
        {
            return _baseRepository.GetFilteredMarkersAsync(commonFilter, parameter);
        }

        public Task<long> GetAllFilteredCountAsync(CommonFilterParameter commonFilter, IFilterParameter parameter)
        {
            return _baseRepository.GetAllFilteredCountAsync(commonFilter, parameter);
        }

        public Task<bool> LoginExistsAsync(string login)
        {
            return _baseRepository.LoginExistsAsync(login);
        }
    }
}
