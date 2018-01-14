

using System.Threading.Tasks;
using Funkmap.Module.Auth.Models;

namespace Funkmap.Module.Auth.Abstract
{
    public interface IRestoreContextManager
    {
        Task<bool> TryCreateRestoreContextAsync(string loginOrEmail);

        Task<bool> TryConfirmRestoreAsync(string loginOrEmail, string code, string newPassword);
    }
}
