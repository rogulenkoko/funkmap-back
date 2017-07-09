using System.Collections.Generic;
using System.Threading.Tasks;
using Funkmap.Data.Entities.Abstract;
using Funkmap.Data.Parameters;

namespace Funkmap.Data.Repositories.Abstract
{
    public interface IBaseRepository
    {
        Task<ICollection<BaseEntity>> GetAllAsyns();

        Task<ICollection<BaseEntity>> GetNearestAsync(LocationParameter parameter);

        Task<ICollection<BaseEntity>> GetFullNearestAsync(FullLocationParameter parameter);

        Task<ICollection<BaseEntity>> GetSpecificAsync(string[] logins);

        Task<ICollection<string>> GetUserEntitiesLogins(string userLogin);

        Task<ICollection<BaseEntity>> GetFilteredAsync(CommonFilterParameter commonFilter, IFilterParameter parameter);
    }
}
