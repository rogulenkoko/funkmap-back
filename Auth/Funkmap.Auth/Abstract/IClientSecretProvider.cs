
namespace Funkmap.Auth.Abstract
{
    /// <summary>
    /// Provider for client credentials
    /// </summary>
    public interface IClientSecretProvider
    {
        /// <summary>
        /// Validate client
        /// </summary>
        /// <param name="clientId">Clinet Id</param>
        /// <param name="clientSecret">Client secret</param>
        bool ValidateClient(string clientId, string clientSecret);
    }
}
