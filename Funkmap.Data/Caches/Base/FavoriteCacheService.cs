using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.Redis;

namespace Funkmap.Data.Caches.Base
{
    public interface IFavoriteCacheService
    {
        void SetFavorites(string userLogin, List<string> logins);
        List<string> GetFavoriteLogins(string userLogin);

    }
    public class FavoriteCacheService : IFavoriteCacheService
    {
        private static readonly string FavouriteKey = "Favorite";

        private readonly IRedisClient _redisStorage;

        public FavoriteCacheService(IRedisClient redisStorage)
        {
            _redisStorage = redisStorage;
        }

        public List<string> GetFavoriteLogins(string userLogin)
        {
            var key = BuildKey(userLogin);
            var logins = _redisStorage.Get<List<string>>(key);
            return logins;
        }

        public void SetFavorites(string userLogin, List<string> logins)
        {
            var key = BuildKey(userLogin);
            _redisStorage.Replace(key, logins);
        }

        private string BuildKey(string userLogin)
        {
            return $"{FavouriteKey}_{userLogin}";
        }
    }
}
