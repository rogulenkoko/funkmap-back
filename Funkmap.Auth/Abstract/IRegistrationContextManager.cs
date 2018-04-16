using System.Threading.Tasks;
using Funkmap.Common.Cqrs;
using Funkmap.Module.Auth.Models;

namespace Funkmap.Module.Auth.Abstract
{
    public interface IRegistrationContextManager
    {
        Task<CommandResponse> TryCreateContextAsync(RegistrationRequest creds);

        Task<CommandResponse> TryConfirmAsync(string login, string email, string code);

    }
}
