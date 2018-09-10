using System.Threading.Tasks;
using Funkmap.Auth.Contracts;
using Funkmap.Common.Models;

namespace Funkmap.Auth.Abstract
{
    public interface IRegistrationContextManager
    {
        Task<BaseResponse> TryCreateContextAsync(RegistrationRequest creds);

        Task<BaseResponse> TryConfirmAsync(string login, string email, string code);

    }
}
