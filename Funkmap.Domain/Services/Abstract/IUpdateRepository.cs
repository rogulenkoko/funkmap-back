using System.Threading.Tasks;
using Funkmap.Domain.Models;

namespace Funkmap.Domain.Services.Abstract
{
    public interface IUpdateRepository
    {
        Task Create(Profile model);
        Task UpdateAsync(Profile model);
    }
}
