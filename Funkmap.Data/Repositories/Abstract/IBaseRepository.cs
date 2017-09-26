using System.Collections.Generic;
using System.Threading.Tasks;
using Funkmap.Common;
using Funkmap.Common.Data.Mongo.Abstract;
using Funkmap.Data.Entities.Abstract;
using Funkmap.Data.Objects;
using Funkmap.Data.Parameters;

namespace Funkmap.Data.Repositories.Abstract
{
    public interface IBaseRepository : IMongoRepository<BaseEntity>
    {
        Task<ICollection<BaseEntity>> GetAllAsyns();

        Task<ICollection<BaseEntity>> GetNearestAsync(LocationParameter parameter);

        Task<ICollection<BaseEntity>> GetFullNearestAsync(FullLocationParameter parameter);

        Task<ICollection<BaseEntity>> GetSpecificAsync(string[] logins);

        Task<ICollection<string>> GetUserEntitiesLogins(string userLogin);

        Task<ICollection<BaseEntity>> GetFilteredAsync(CommonFilterParameter commonFilter, IFilterParameter parameter = null);

        Task<ICollection<string>> GetAllFilteredLoginsAsync(CommonFilterParameter commonFilter, IFilterParameter parameter);

        Task<bool> CheckIfLoginExistAsync(string login);

        Task UpdateAsync(BaseEntity entity);

        Task<ICollection<UserEntitiesCountInfo>> GetUserEntitiesCountInfo(string userLogin);
    }
}
