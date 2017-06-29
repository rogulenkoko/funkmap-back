using System.Collections.Generic;
using System.Threading.Tasks;
using Funkmap.Data.Domain;
using Funkmap.Data.Entities;
using Funkmap.Data.Parameters;

namespace Funkmap.Data.Repositories.Abstract
{
    public interface IBaseRepository
    {
        Task<ICollection<BaseEntity>> GetAllAsyns();

        Task<ICollection<BaseEntity>> GetNearestAsync(LocationParameter parameter);

        Task<ICollection<BaseEntity>> GetFullNearestAsync(FullLocationParameter parameter);
    }
}
