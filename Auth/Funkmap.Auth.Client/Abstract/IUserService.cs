using System.Threading.Tasks;
using Funkmap.Auth.Contracts;

namespace Funkmap.Auth.Client.Abstract
{
    /// <summary>
    /// User service
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Get user
        /// </summary>
        /// <param name="login">Login</param>
        Task<UserResponse> GetUserAsync(string login);
    }
}
