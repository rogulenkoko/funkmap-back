using System.Threading.Tasks;

namespace Funkmap.Auth.Abstract
{
    public interface IRestoreContextManager
    {
        Task<bool> TryCreateRestoreContextAsync(string loginOrEmail);

        Task<bool> TryConfirmRestoreAsync(string loginOrEmail, string code, string newPassword);
    }
}
