using System;
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
        Task<string> SaveAvatarAsync(string login, byte[] image);

        Task UpdateLastVisitDateAsync(string login, DateTime date);

        Task<DateTime?> GetLastVisitDate(string login);

        Task<UserEntity> GetUserByEmail(string email);
    }
}
