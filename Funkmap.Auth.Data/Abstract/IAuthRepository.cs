using System.Threading.Tasks;
using Funkmap.Auth.Data.Entities;
using Funkmap.Common.Data.Mongo.Abstract;

namespace Funkmap.Auth.Data.Abstract
{
    public interface IAuthRepository : IMongoRepository<UserEntity>
    {
        Task<UserEntity> Login(string login, string password);

        Task<bool> CheckIfExist(string login);

        Task<byte[]> GetAvatarAsync(string login);
        Task<bool> SaveAvatarAsync(string login, byte[] image);
    }
}
