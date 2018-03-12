using System.Threading.Tasks;
using Funkmap.Auth.Data.Entities;

namespace Funkmap.Module.Auth.Services.External.Abstract
{
    public interface IExternalAuthService
    {
        AuthProviderType ProviderType { get; }

        Task<UserEntity> BuildUserAsync(string token);
    }
}
