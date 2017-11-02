using System.Collections.Generic;
using System.Threading.Tasks;
using Funkmap.Common.Redis.Abstract;

namespace Funkmap.Data.Caches.Base
{
    public interface IFavoriteCacheService
    {
        Task SetFavorites(string userLogin, List<string> logins);
        Task<List<string>> GetFavoriteLogins(string userLogin);

    }
    public class FavoriteCacheService : IFavoriteCacheService
    {
        private static readonly string FavouriteKey = "Favorite";

        private readonly IStorage _storage;

        public FavoriteCacheService(IStorage storage)
        {
            _storage = storage;
        }

        public async Task<List<string>> GetFavoriteLogins(string userLogin)
        {
            var key = BuildKey(userLogin);
            var logins = await _storage.GetAsync<List<string>>(key);
            return logins;
        }

        public async Task SetFavorites(string userLogin, List<string> logins)
        {
            var key = BuildKey(userLogin);
            await _storage.SetAsync(key, logins);
        }

        private string BuildKey(string userLogin)
        {
            return $"{FavouriteKey}_{userLogin}";
        }
    }
}
