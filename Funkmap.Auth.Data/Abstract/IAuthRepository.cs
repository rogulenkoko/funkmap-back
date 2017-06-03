using System.Threading.Tasks;
using Funkmap.Auth.Data.Entities;
using Funkmap.Common.Data.Abstract;

namespace Funkmap.Auth.Data.Abstract
{
    public interface IAuthRepository : IRepository<UserEntity>
    {
        Task<UserEntity> Login(string login, string password);

        Task<bool> CheckIfExist(string login);
    }
}
