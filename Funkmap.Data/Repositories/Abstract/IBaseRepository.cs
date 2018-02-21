using System.Collections.Generic;
using System.Threading.Tasks;
using Funkmap.Common.Data.Mongo.Abstract;
using Funkmap.Data.Entities.Entities.Abstract;
using Funkmap.Data.Objects;
using Funkmap.Data.Parameters;

namespace Funkmap.Data.Repositories.Abstract
{
    public interface IBaseRepository : IMongoRepository<BaseEntity>
    {
        Task<List<BaseEntity>> GetNearestAsync(LocationParameter parameter);

        Task<List<BaseEntity>> GetFullNearestAsync(LocationParameter parameter);

        Task<List<BaseEntity>> GetSpecificNavigationAsync(IReadOnlyCollection<string> logins);

        Task<List<BaseEntity>> GetSpecificFullAsync(IReadOnlyCollection<string> logins);

        Task<List<string>> GetUserEntitiesLoginsAsync(string userLogin);

        Task<List<BaseEntity>> GetFilteredAsync(CommonFilterParameter commonFilter, IFilterParameter parameter = null);

        Task<List<BaseEntity>> GetFilteredNavigationAsync(CommonFilterParameter commonFilter, IFilterParameter parameter = null);

        Task<long> GetAllFilteredCountAsync(CommonFilterParameter commonFilter, IFilterParameter parameter);

        Task<bool> CheckIfLoginExistAsync(string login);

        Task<List<UserEntitiesCountInfo>> GetUserEntitiesCountInfoAsync(string userLogin);

        Task<byte[]> GetFileAsync(string fileId);

        Task UpdateFavoriteAsync(UpdateFavoriteParameter parameter);

        Task<List<string>> GetFavoritesLoginsAsync(string userLogin);

        Task UpdateAvatarAsync(BaseEntity entity, byte[] imageBytes);
    }
}
