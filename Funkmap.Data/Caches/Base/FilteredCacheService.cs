using System;
using System.Collections.Generic;
using System.Text;
using Funkmap.Data.Parameters;
using ServiceStack.Redis;

namespace Funkmap.Data.Caches.Base
{
    public interface IFilteredCacheService
    {
        ICollection<string> GetFilteredLogins(CommonFilterParameter commonFilter, IFilterParameter parameter);
        void SetFilteredLogins(CommonFilterParameter commonFilter, IFilterParameter parameter, List<string> logins);
    }
    public class FilteredCacheService : IFilteredCacheService
    {
        private static readonly string FilteredKey = "Filtered";
        private static readonly TimeSpan ExpirationTime = TimeSpan.FromMinutes(2);

        private readonly IRedisClient _redisStorage;

        public FilteredCacheService(IRedisClient redisStorage)
        {
            _redisStorage = redisStorage;
        }

        public ICollection<string> GetFilteredLogins(CommonFilterParameter commonFilter, IFilterParameter parameter)
        {

            var key = BuildKey(commonFilter, parameter);
            ICollection<string> logins = _redisStorage.Get<List<string>>(key);

            _redisStorage.ExpireEntryIn(key, ExpirationTime);

            return logins;
        }

        public void SetFilteredLogins(CommonFilterParameter commonFilter, IFilterParameter parameter, List<string> logins)
        {
            var key = BuildKey(commonFilter, parameter);
            _redisStorage.Add(key, logins);
            _redisStorage.ExpireEntryIn(key, ExpirationTime);
        }

        private string BuildKey(CommonFilterParameter commonFilter, IFilterParameter parameter)
        {
            var sb = new StringBuilder();
            sb.Append(commonFilter);
            if (parameter != null)
            {
                sb.Append(parameter);
            }

            var key = sb.ToString();
            return key;
        }
    }
}
