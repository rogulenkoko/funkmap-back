using System;
using System.Threading.Tasks;
using Funkmap.Common.Abstract;
using Funkmap.Common.Serialization;
using StackExchange.Redis;

namespace Funkmap.Common.Redis
{
    public class RedisStorage : IStorage
    {
        private readonly IDatabase _database;
        private readonly ISerializer _serializer;

        public RedisStorage(IDatabase database, ISerializer serializer)
        {
            _database = database;
            _serializer = serializer;
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? lifeTime = null) where T : class
        {
            var serialized = _serializer.Serialize(value);
            await _database.StringSetAsync(key, serialized, lifeTime);
        }

        public async Task<T> GetAsync<T>(string key) where T : class
        {
            var value = await _database.StringGetAsync(key);
            return _serializer.Deserialize<T>(value);
        }

        public async Task<bool> RemoveAsync(string key)
        {
            return await _database.KeyDeleteAsync(key);
        }
    }
}
