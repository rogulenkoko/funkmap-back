using Funkmap.Auth.Contracts;

namespace Funkmap.Auth.Services
{
    /// <summary>
    /// External authorization service
    /// </summary>
    public interface ISocialUserService
    {
        /// <summary>
        /// External authorization provider name
        /// </summary>
        string Provider { get; }

        /// <summary>
        /// Try get user from external service
        /// </summary>
        /// <param name="token">External service authorization token</param>
        /// <param name="user">External service user</param>
        bool TryGetUser(string token, out User user);
    }
}