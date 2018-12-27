using System.Threading.Tasks;
using Funkmap.Auth.Contracts;
using Funkmap.Common.Models;

namespace Funkmap.Auth.Abstract
{
    /// <summary>
    /// Context manager for user registration
    /// </summary>
    public interface IRegistrationContextManager
    {
        /// <summary>
        /// Cteacte registration context
        /// </summary>
        /// <param name="creds"><see cref="RegistrationRequest"/></param>
        Task<BaseResponse> TryCreateContextAsync(RegistrationRequest creds);

        /// <summary>
        /// Confirm registration
        /// </summary>
        /// <param name="login">Login</param>
        /// <param name="email">Email</param>
        /// <param name="code">Confirmation code</param>
        Task<BaseResponse> TryConfirmAsync(string login, string email, string code);

    }
}
