using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Funkmap.Auth.Data.Abstract;
using Funkmap.Auth.Data.Entities;
using Funkmap.Auth.Data.Objects;
using Funkmap.Common.Data.Mongo;
using MongoDB.Driver;

namespace Funkmap.Auth.Data
{
    public class AuthRepository : MongoLoginRepository<UserEntity>, IAuthRepository
    {
        public AuthRepository(IMongoCollection<UserEntity> collection) : base(collection)
        {
        }

        public async Task<UserEntity> Login(string login, string password)
        {
            var user = await _collection.Find(x=>x.Login == login && x.Password == password).SingleOrDefaultAsync();
            return user;
        }

        public async Task<bool> CheckIfExist(string login)
        {
            var projection = Builders<UserEntity>.Projection.Include(x => x.Login);
            var userId = await _collection.Find(x => x.Login == login).Project(projection).SingleOrDefaultAsync();
            var isExist = userId != null;
            return isExist;
        }

        public async Task<ICollection<UserAvatarResult>> GetAvatarsAsync(string[] logins)
        {
            var projection = Builders<UserEntity>.Projection.Include(x => x.Avatar).Include(x=>x.Login);
            var filter = Builders<UserEntity>.Filter.In(x => x.Login, logins);
            var avatars = await _collection.Find(filter).Project<UserEntity>(projection).ToListAsync();
            return avatars.Select(x=> new UserAvatarResult() {Login = x.Login, Avatar = x.Avatar?.AsByteArray}).ToList();
        }

        public async Task SaveAvatarAsync(string login, byte[] image)
        {
            var filter = Builders<UserEntity>.Filter.Eq(x => x.Login, login);
            var update = Builders<UserEntity>.Update.Set(x => x.Avatar, image);
            await _collection.UpdateOneAsync(filter, update);
        }

        public async Task<List<string>> GetFavouritesAsync(string login)
        {
            var projection = Builders<UserEntity>.Projection.Include(x => x.Favourites);
            var user = await _collection.Find(x => x.Login == login).Project<UserEntity>(projection).SingleOrDefaultAsync();
            return user?.Favourites;
        }

        public async Task SetFavourite(string login, string favouriteLogin)
        {
            var projection = Builders<UserEntity>.Projection.Include(x => x.Favourites);
            var user = await _collection.Find(x => x.Login == login).Project<UserEntity>(projection).SingleOrDefaultAsync();
            if (user == null) throw new InvalidOperationException(login);
            var filter = Builders<UserEntity>.Filter.Eq(x => x.Login, login);
            var update = user.Favourites.Contains(favouriteLogin) 
                         ? Builders<UserEntity>.Update.Pull(x => x.Favourites, favouriteLogin)
                         : Builders<UserEntity>.Update.Push(x => x.Favourites, favouriteLogin);
            await _collection.UpdateOneAsync(filter, update);
        }

        public async Task UpdateLastVisitDateAsync(string login, DateTime date)
        {
            var update = Builders<UserEntity>.Update.Set(x => x.LastVisitDateUtc, date);
            await _collection.UpdateOneAsync(x => x.Login == login, update);
        }

        public async Task<DateTime?> GetLastVisitDate(string login)
        {
            var dateProjection = Builders<UserEntity>.Projection.Include(x => x.LastVisitDateUtc);
            var user = await _collection.Find(x => x.Login == login)
                .Project<UserEntity>(dateProjection)
                .SingleOrDefaultAsync();
            return user?.LastVisitDateUtc;
        }

        public override Task UpdateAsync(UserEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
