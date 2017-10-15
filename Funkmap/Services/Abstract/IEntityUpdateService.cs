using System.Threading.Tasks;
using Funkmap.Models;

namespace Funkmap.Services.Abstract
{
    public interface IEntityUpdateService
    {
        Task CreateEntity(BaseModel model);
        Task UpdateEntity(BaseModel model);
    }
}
