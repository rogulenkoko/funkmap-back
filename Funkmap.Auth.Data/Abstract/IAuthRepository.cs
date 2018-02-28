using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Funkmap.Auth.Data.Entities;
using Funkmap.Common.Data.Mongo.Abstract;

namespace Funkmap.Auth.Data.Abstract
{
    public interface IAuthRepository : IMongoRepository<UserEntity>
    {
        Task<UserEntity> LoginAsync(string login, string password);

        Task<bool> CheckIfExistAsync(string login);

        Task<byte[]> GetAvatarAsync(string login);
        Task<string> SaveAvatarAsync(string login, byte[] image);

        Task UpdateLastVisitDateAsync(string login, DateTime date);

        Task UpdateLocaleAsync(string login, string locale);

        Task<DateTime?> GetLastVisitDateAsync(string login);

        Task<List<string>> GetBookedEmailsAsync();

        Task<UserEntity> GetUserByEmailOrLoginAsync(string emailOrLogin);
    }
}
