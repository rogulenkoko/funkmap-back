using System.Threading.Tasks;
using Funkmap.Auth.Data.Entities;
using Funkmap.Module.Auth.Models;

namespace Funkmap.Module.Auth.Abstract
{
    public interface IRegistrationContextManager
    {
        Task<bool> ValidateLogin(string login);
        Task<bool> Validate(string login, string email);
        Task<bool> TryCreateContextAsync(RegistrationRequest creds);

        Task<bool> TryConfirmAsync(string login, string email, string code);

        Task<bool> TryRegisterExternal(UserEntity user);

    }
}
