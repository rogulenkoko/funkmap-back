using System.Threading.Tasks;
using Funkmap.Domain.Models;

namespace Funkmap.Domain.Services.Abstract
{
    public interface IAccessService
    {
        Task<CanCreateProfileResult> CanCreateProfileAsync(string userLogin);
    }
}
