using System.Threading.Tasks;
using Funkmap.Domain.Models;

namespace Funkmap.Domain.Services.Abstract
{
    public interface IEntityUpdateService
    {
        Task CreateEntity(Profile model);
        Task UpdateEntity(Profile model);
    }
}
