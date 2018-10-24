using System.Threading.Tasks;
using Funkmap.Domain.Models;

namespace Funkmap.Domain.Abstract.Repositories
{
    public interface IProAccountRepository
    {
        Task CreateAsync(ProAccount proAccount);

        Task<ProAccount> GetAsync(string userLogin);
    }
}
