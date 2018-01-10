using System;
using System.Timers;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Funkmap.Common.Redis.Abstract;

namespace Funkmap.Common.Tools
{
    public class InMemoryStorage : IStorage
    {
        private static readonly ConcurrentDictionary<string, object> _cache = new ConcurrentDictionary<string, object>();
        private static readonly ICollection<Timer> _timers = new List<Timer>();

        public async Task SetAsync<T>(string key, T value, TimeSpan? lifeTime = null) where T : class
        {
            await Task.Yield();

            _cache.AddOrUpdate(key, (object)value, (k, v) => value);
            if (lifeTime.HasValue)
            {
                var timer = new Timer(lifeTime.Value.TotalMilliseconds);
                object removedValue;
                timer.Elapsed += (sender, args) =>
                {
                    _cache.TryRemove(key, out removedValue);
                    _timers.Remove(timer);
                };
                _timers.Add(timer);
            }
            
        }

        public async Task<T> GetAsync<T>(string key) where T : class
        {
            await Task.Yield();

            object value;
            _cache.TryGetValue(key, out value);
            return (T) value;
        }
    }
}
