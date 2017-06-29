using System.Collections.Generic;
using System.Threading.Tasks;
using Funkmap.Data.Domain;
using Funkmap.Data.Entities.Abstract;
using Funkmap.Data.Parameters;

namespace Funkmap.Data.Repositories.Abstract
{
    public interface IBaseRepository
    {
        Task<ICollection<BaseEntity>> GetAllAsyns();

        Task<ICollection<BaseEntity>> GetNearestAsync(LocationParameter parameter);

        Task<ICollection<FullBaseModel>> GetFullNearest(LocationParameter parameter);
    }
}
