using System.Threading.Tasks;
using Funkmap.Auth.Models;
using Funkmap.Common.Cqrs;

namespace Funkmap.Auth.Abstract
{
    public interface IRegistrationContextManager
    {
        Task<CommandResponse> TryCreateContextAsync(RegistrationRequest creds);

        Task<CommandResponse> TryConfirmAsync(string login, string email, string code);

    }
}
