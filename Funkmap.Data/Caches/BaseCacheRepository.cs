using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Data.Entities.Abstract;
using Funkmap.Data.Objects;
using Funkmap.Data.Parameters;
using Funkmap.Data.Repositories.Abstract;
using MongoDB.Driver;
using ServiceStack.Redis;

namespace Funkmap.Data.Caches
{
    public class BaseCacheRepository : IBaseRepository
    {
        private readonly IRedisClient _redisStorage;
        private readonly IBaseRepository _baseRepository;

        public BaseCacheRepository(IRedisClient redisStorage, IBaseRepository baseRepository)
        {
            _redisStorage = redisStorage;
            _baseRepository = baseRepository;

        }

        #region Update

        public Task CreateAsync(BaseEntity item)
        {
            return _baseRepository.CreateAsync(item);
        }

        public Task<DeleteResult> DeleteAsync(string id)
        {
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

        public Task<ICollection<UserEntitiesCountInfo>> GetUserEntitiesCountInfo(string userLogin)
        {
            return _baseRepository.GetUserEntitiesCountInfo(userLogin);
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

        public Task<ICollection<string>> GetUserEntitiesLogins(string userLogin)
        {
            return _baseRepository.GetUserEntitiesLogins(userLogin);
        }

        public Task<ICollection<BaseEntity>> GetFilteredAsync(CommonFilterParameter commonFilter, IFilterParameter parameter = null)
        {
            return _baseRepository.GetFilteredAsync(commonFilter, parameter);
        }

        public async Task<ICollection<string>> GetAllFilteredLoginsAsync(CommonFilterParameter commonFilter, IFilterParameter parameter)
        {
            var sb = new StringBuilder();
            sb.Append(commonFilter);
            if (parameter != null)
            {
                sb.Append(parameter);
            }

            var key = sb.ToString();
            var lifeTime = TimeSpan.FromMinutes(2);

            ICollection<string> logins = _redisStorage.Get<List<string>>(key);
            if (logins == null)
            {
                var result = await _baseRepository.GetAllFilteredLoginsAsync(commonFilter, parameter);
                if (result.Count == 0)
                    return null;
                _redisStorage.Set(key, result,lifeTime);
                //_redisStorage.ExpireEntryIn(key, lifeTime);
                return result;
            }
            _redisStorage.ExpireEntryIn(key, lifeTime);
            return logins;
        }

        public Task<bool> CheckIfLoginExistAsync(string login)
        {
            return _baseRepository.CheckIfLoginExistAsync(login);
        }
    }
}
