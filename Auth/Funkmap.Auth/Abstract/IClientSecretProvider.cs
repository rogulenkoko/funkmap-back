
namespace Funkmap.Auth.Abstract
{
    public interface IClientSecretProvider
    {
        bool ValidateClient(string clientId, string clientSecret);
    }
}
