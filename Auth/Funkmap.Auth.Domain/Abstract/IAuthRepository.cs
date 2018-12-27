using System.Collections.Generic;
using System.Threading.Tasks;
using Funkmap.Auth.Contracts;
using Funkmap.Common.Models;

namespace Funkmap.Auth.Domain.Abstract
{
    /// <summary>
    /// Repository for authorization data
    /// </summary>
    public interface IAuthRepository
    {
        /// <summary>
        /// Get user
        /// </summary>
        /// <param name="login">User's login</param>
        Task<User> GetAsync(string login);

        /// <summary>
        /// Get booked emails
        /// </summary>
        Task<List<string>> GetBookedEmailsAsync();

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="login">User's login</param>
        /// <param name="hashedPassword">Hashed password</param>
        Task<User> LoginAsync(string login, string hashedPassword);

        /// <summary>
        /// Get avatar data
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        Task<byte[]> GetAvatarAsync(string login);

        /// <summary>
        /// Save avatar
        /// </summary>
        /// <param name="login">User's login</param>
        /// <param name="image">Image data</param>
        Task<string> SaveAvatarAsync(string login, byte[] image);

        /// <summary>
        /// Create user
        /// </summary>
        /// <param name="user"><see cref="User"/></param>
        /// <param name="hashedPassword">Hashed password</param>
        Task<BaseResponse> TryCreateAsync(User user, string hashedPassword);

        /// <summary>
        /// Create social user
        /// </summary>
        /// <param name="user"><see cref="User"/></param>
        Task<BaseResponse> TryCreateSocialAsync(User user);

        /// <summary>
        /// Update user's locale
        /// </summary>
        /// <param name="login">User's login</param>
        /// <param name="locale">Locale</param>
        Task UpdateLocaleAsync(string login, string locale);

        /// <summary>
        /// Update user's password
        /// </summary>
        /// <param name="login">User's login</param>
        /// <param name="hashedPassword">Hashed password</param>
        Task UpdatePasswordAsync(string login, string hashedPassword);
    }
}
