using Funkmap.Auth.Data.Entities;
using Funkmap.Common.Data.Abstract;

namespace Funkmap.Auth.Data.Abstract
{
    public interface IAuthRepository : IRepository<UserEntity>
    {
        bool Login(string login, string password);
    }
}
