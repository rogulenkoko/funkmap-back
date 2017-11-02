using System;
using System.Threading.Tasks;

namespace Funkmap.Common.Redis.Abstract
{
    public interface IStorage
    {
        Task SetAsync<T>(string key, T value, TimeSpan? lifeTime = null) where T : class;

        Task<T> GetAsync<T>(string key) where T : class;
    }
}
