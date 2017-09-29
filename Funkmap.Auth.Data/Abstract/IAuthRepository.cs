using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Funkmap.Auth.Data.Entities;
using Funkmap.Auth.Data.Objects;
using Funkmap.Common.Data.Mongo.Abstract;

namespace Funkmap.Auth.Data.Abstract
{
    public interface IAuthRepository : IMongoRepository<UserEntity>
    {
        Task<UserEntity> Login(string login, string password);

        Task<bool> CheckIfExist(string login);

        Task<ICollection<UserAvatarResult>> GetAvatarsAsync(string[] login);
        Task SaveAvatarAsync(string login, byte[] image);

        Task<List<string>> GetFavouritesAsync(string login);

        Task SetFavourite(string login, string favouriteLogin);

        Task UpdateLastVisitDateAsync(string login, DateTime date);

        Task<DateTime?> GetLastVisitDate(string login);

        Task<UserEntity> GetUserByEmail(string email);
    }
}
