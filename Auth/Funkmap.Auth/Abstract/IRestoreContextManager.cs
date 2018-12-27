using System.Threading.Tasks;

namespace Funkmap.Auth.Abstract
{
    /// <summary>
    /// Context manager for password restore
    /// </summary>
    public interface IRestoreContextManager
    {
        /// <summary>
        /// Create restore context
        /// </summary>
        /// <param name="loginOrEmail">Login or email</param>
        /// <returns></returns>
        Task<bool> TryCreateRestoreContextAsync(string loginOrEmail);

        /// <summary>
        /// Restore password
        /// </summary>
        /// <param name="loginOrEmail">Login or email</param>
        /// <param name="code">Confirmation code</param>
        /// <param name="newPassword">New password</param>
        Task<bool> TryConfirmRestoreAsync(string loginOrEmail, string code, string newPassword);
    }
}
