using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Data.Entities.Abstract;
using Funkmap.Data.Objects;
using Funkmap.Data.Parameters;
using Funkmap.Data.Repositories.Abstract;

namespace Funkmap.Data.Caches.Base
{
    public class BaseCacheRepository : IBaseRepository
    {
        private readonly IBaseRepository _baseRepository;
        private readonly IFavoriteCacheService _favoriteService;
        private readonly IFilteredCacheService _filteredService;

        public BaseCacheRepository(IFavoriteCacheService favoriteCacheService, IFilteredCacheService filteredCacheService, IBaseRepository baseRepository)
        {
            _baseRepository = baseRepository;
            _favoriteService = favoriteCacheService;
            _filteredService = filteredCacheService;

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


        public Task<ICollection<BaseEntity>> GetAllAsync()
        {
            return _baseRepository.GetAllAsyns();
        }

        public Task<BaseEntity> GetAsync(string id)
        {
            return _baseRepository.GetAsync(id);
        }

        public Task<ICollection<UserEntitiesCountInfo>> GetUserEntitiesCountInfoAsync(string userLogin)
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
            _favoriteService.SetFavorites(parameter.UserLogin, favorites.ToList());

        }

        public async Task<ICollection<string>> GetFavoritesLoginsAsync(string userLogin)
        {
            ICollection<string> favorites = await _favoriteService.GetFavoriteLogins(userLogin);
            if (favorites == null)
            {
                favorites = await _baseRepository.GetFavoritesLoginsAsync(userLogin);
                _favoriteService.SetFavorites(userLogin, favorites as List<string>);
            }
            return favorites;

        }

        public Task UpdateAvatarAsync(BaseEntity entity, byte[] imageBytes)
        {
            return _baseRepository.UpdateAvatarAsync(entity, imageBytes);
        }

        public Task<ICollection<BaseEntity>> GetAllAsyns()
        {
            return _baseRepository.GetAllAsyns();
        }

        public Task<ICollection<BaseEntity>> GetNearestAsync(LocationParameter parameter)
        {
            return _baseRepository.GetNearestAsync(parameter);
        }

        public Task<ICollection<BaseEntity>> GetFullNearestAsync(LocationParameter parameter)
        {
            return _baseRepository.GetFullNearestAsync(parameter);
        }

        public Task<ICollection<BaseEntity>> GetSpecificNavigationAsync(string[] logins)
        {
            return _baseRepository.GetSpecificNavigationAsync(logins);
        }

        public Task<ICollection<BaseEntity>> GetSpecificFullAsync(string[] logins)
        {
            return _baseRepository.GetSpecificFullAsync(logins);
        }

        public Task<ICollection<string>> GetUserEntitiesLoginsAsync(string userLogin)
        {
            return _baseRepository.GetUserEntitiesLoginsAsync(userLogin);
        }

        public Task<ICollection<BaseEntity>> GetFilteredAsync(CommonFilterParameter commonFilter, IFilterParameter parameter = null)
        {
            return _baseRepository.GetFilteredAsync(commonFilter, parameter);
        }

        public async Task<ICollection<string>> GetAllFilteredLoginsAsync(CommonFilterParameter commonFilter, IFilterParameter parameter)
        {
            var logins = await _filteredService.GetFilteredLogins(commonFilter, parameter);
            if (logins == null)
            {
                var result = await _baseRepository.GetAllFilteredLoginsAsync(commonFilter, parameter);
                _filteredService.SetFilteredLogins(commonFilter, parameter, result as List<string>);
                return result;
            }
           
            return logins;
        }

        public Task<bool> CheckIfLoginExistAsync(string login)
        {
            return _baseRepository.CheckIfLoginExistAsync(login);
        }
    }
}
