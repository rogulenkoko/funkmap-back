using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Common.Redis.Abstract;
using Funkmap.Data.Parameters;

namespace Funkmap.Data.Caches.Base
{
    public interface IFilteredCacheService
    {
        Task<ICollection<string>> GetFilteredLogins(CommonFilterParameter commonFilter, IFilterParameter parameter);
        Task SetFilteredLogins(CommonFilterParameter commonFilter, IFilterParameter parameter, List<string> logins);
    }
    public class FilteredCacheService : IFilteredCacheService
    {
        private static readonly string FilteredKey = "Filtered";
        private static readonly TimeSpan ExpirationTime = TimeSpan.FromMinutes(2);

        private readonly IStorage _storage;

        public FilteredCacheService(IStorage storage)
        {
            _storage = storage;
        }

        public async Task<ICollection<string>> GetFilteredLogins(CommonFilterParameter commonFilter, IFilterParameter parameter)
        {

            var key = BuildKey(commonFilter, parameter);
            ICollection<string> logins = await _storage.GetAsync<List<string>>(key);

            return logins;
        }

        public async Task SetFilteredLogins(CommonFilterParameter commonFilter, IFilterParameter parameter, List<string> logins)
        {
            var key = BuildKey(commonFilter, parameter);
            await _storage.SetAsync(key, logins, ExpirationTime);
        }

        private string BuildKey(CommonFilterParameter commonFilter, IFilterParameter parameter)
        {
            var sb = new StringBuilder();
            sb.Append($"{FilteredKey}_");
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
