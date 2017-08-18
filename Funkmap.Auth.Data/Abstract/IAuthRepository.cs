using System;
using System.Collections.Generic;
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
        Task SaveAvatarAsync(string login, byte[] image);

        Task<List<string>> GetFavouritesAsync(string login);

        Task SetFavourite(string login, string favouriteLogin);

        Task UpdateLastVisitDate(string login, DateTime date);
    }
}
