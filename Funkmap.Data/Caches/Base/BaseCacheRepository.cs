using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Funkmap.Data.Entities.Entities.Abstract;
using Funkmap.Data.Objects;
using Funkmap.Data.Parameters;
using Funkmap.Data.Repositories.Abstract;

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

        #region Update

        public Task CreateAsync(BaseEntity item)
        {
            return _baseRepository.CreateAsync(item);
        }

        public Task<BaseEntity> DeleteAsync(string id)
        {
            //todo решить как быть здесь с избранными
            return _baseRepository.DeleteAsync(id);
        }

        public Task UpdateAsync(BaseEntity entity)
        {
            return _baseRepository.UpdateAsync(entity);
        }

        #endregion


        public Task<List<BaseEntity>> GetAllAsync()
        {
            return _baseRepository.GetAllAsync();
        }

        public Task<BaseEntity> GetAsync(string id)
        {
            return _baseRepository.GetAsync(id);
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

        public Task UpdateAvatarAsync(BaseEntity entity, byte[] imageBytes)
        {
            return _baseRepository.UpdateAvatarAsync(entity, imageBytes);
        }

        public Task<List<BaseEntity>> GetAllAsyns()
        {
            return _baseRepository.GetAllAsync();
        }

        public Task<List<BaseEntity>> GetNearestAsync(LocationParameter parameter)
        {
            return _baseRepository.GetNearestAsync(parameter);
        }

        public Task<List<BaseEntity>> GetFullNearestAsync(LocationParameter parameter)
        {
            return _baseRepository.GetFullNearestAsync(parameter);
        }

        public Task<List<BaseEntity>> GetSpecificNavigationAsync(IReadOnlyCollection<string> logins)
        {
            return _baseRepository.GetSpecificNavigationAsync(logins);
        }

        public Task<List<BaseEntity>> GetSpecificFullAsync(IReadOnlyCollection<string> logins)
        {
            return _baseRepository.GetSpecificFullAsync(logins);
        }

        public Task<List<string>> GetUserEntitiesLoginsAsync(string userLogin)
        {
            return _baseRepository.GetUserEntitiesLoginsAsync(userLogin);
        }

        public Task<List<BaseEntity>> GetFilteredAsync(CommonFilterParameter commonFilter, IFilterParameter parameter = null)
        {
            return _baseRepository.GetFilteredAsync(commonFilter, parameter);
        }

        public Task<List<BaseEntity>> GetFilteredNavigationAsync(CommonFilterParameter commonFilter, IFilterParameter parameter = null)
        {
            return _baseRepository.GetFilteredNavigationAsync(commonFilter, parameter);
        }

        public Task<long> GetAllFilteredCountAsync(CommonFilterParameter commonFilter, IFilterParameter parameter)
        {
            return _baseRepository.GetAllFilteredCountAsync(commonFilter, parameter);
        }

        public Task<bool> CheckIfLoginExistAsync(string login)
        {
            return _baseRepository.CheckIfLoginExistAsync(login);
        }
    }
}
