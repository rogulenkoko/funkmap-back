using System;
using System.Threading.Tasks;

namespace Funkmap.Common.Abstract
{
    public interface IStorage
    {
        Task SetAsync<T>(string key, T value, TimeSpan? lifeTime = null) where T : class;

        Task<T> GetAsync<T>(string key) where T : class;

        Task<bool> RemoveAsync(string key);
    }
}
