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
        Task<ICollection<BaseEntity>> GetAllAsyns();

        Task<ICollection<BaseEntity>> GetNearestAsync(LocationParameter parameter);

        Task<ICollection<BaseEntity>> GetFullNearestAsync(LocationParameter parameter);

        Task<ICollection<BaseEntity>> GetSpecificNavigationAsync(string[] logins);

        Task<ICollection<BaseEntity>> GetSpecificFullAsync(string[] logins);

        Task<ICollection<string>> GetUserEntitiesLoginsAsync(string userLogin);

        Task<ICollection<BaseEntity>> GetFilteredAsync(CommonFilterParameter commonFilter, IFilterParameter parameter = null);

        Task<ICollection<BaseEntity>> GetFilteredNavigationAsync(CommonFilterParameter commonFilter, IFilterParameter parameter = null);

        Task<long> GetAllFilteredCountAsync(CommonFilterParameter commonFilter, IFilterParameter parameter);

        Task<bool> CheckIfLoginExistAsync(string login);

        Task<ICollection<UserEntitiesCountInfo>> GetUserEntitiesCountInfoAsync(string userLogin);

        Task<byte[]> GetFileAsync(string fileId);

        Task UpdateFavoriteAsync(UpdateFavoriteParameter parameter);

        Task<ICollection<string>> GetFavoritesLoginsAsync(string userLogin);

        Task UpdateAvatarAsync(BaseEntity entity, byte[] imageBytes);
    }
}
