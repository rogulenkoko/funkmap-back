using System.Threading.Tasks;
using Funkmap.Auth.Contracts;

namespace Funkmap.Auth.Client.Abstract
{
    public interface IUserService
    {
        Task<UserResponse> GetUserAsync(string login);
    }
}
