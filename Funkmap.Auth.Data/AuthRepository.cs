using System;
using System.Threading.Tasks;
using Funkmap.Auth.Data.Abstract;
using Funkmap.Auth.Data.Entities;
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

        public async Task<byte[]> GetAvatarAsync(string login)
        {
            var projection = Builders<UserEntity>.Projection.Include(x => x.Avatar);
            var avatar = await _collection.Find(x=>x.Login == login).Project<UserEntity>(projection).SingleOrDefaultAsync();
            return avatar?.Avatar?.AsByteArray;
        }

        public async Task<bool> SaveAvatarAsync(string login, byte[] image)
        {
            var filter = Builders<UserEntity>.Filter.Eq(x => x.Login, login);
            var update = Builders<UserEntity>.Update.Set(x => x.Avatar, image);
            try
            {
                var result = await _collection.UpdateOneAsync(filter, update);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public override Task<UpdateResult> UpdateAsync(UserEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
