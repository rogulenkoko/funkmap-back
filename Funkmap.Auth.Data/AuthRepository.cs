using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Funkmap.Auth.Data.Abstract;
using Funkmap.Auth.Data.Entities;
using Funkmap.Common.Abstract;
using Funkmap.Common.Data.Mongo;
using MongoDB.Driver;
using Autofac.Features.AttributeFilters;

namespace Funkmap.Auth.Data
{
    public class AuthRepository : MongoLoginRepository<UserEntity>, IAuthRepository
    {
        private readonly IFileStorage _fileStorage;

        public AuthRepository(IMongoCollection<UserEntity> collection,
                              [KeyFilter(AuthCollectionNameProvider.AuthStorageName)] IFileStorage fileStorage) : base(collection)
        {
            _fileStorage = fileStorage;
        }

        public async Task<UserEntity> LoginAsync(string login, string password)
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
            var projection = Builders<UserEntity>.Projection.Include(x => x.AvatarId).Include(x=>x.Login);
            var filter = Builders<UserEntity>.Filter.Eq(x => x.Login, login);
            var user = await _collection.Find(filter).Project<UserEntity>(projection).SingleOrDefaultAsync();

            if (String.IsNullOrEmpty(user?.AvatarId)) return null;
            
            try
            {
                var bytes = await _fileStorage.DownloadAsBytesAsync(user.AvatarId);
                return bytes;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<string> SaveAvatarAsync(string login, byte[] image)
        {
            var filename = $"avatar_{login}";
            var fullPath = await _fileStorage.UploadFromBytesAsync(filename, image);

            var filter = Builders<UserEntity>.Filter.Eq(x => x.Login, login);
            var update = Builders<UserEntity>.Update.Set(x => x.AvatarId, fullPath);
            await _collection.UpdateOneAsync(filter, update);
            return fullPath;
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

        public async Task<UserEntity> GetUserByEmail(string email)
        {
            var filter = Builders<UserEntity>.Filter.Eq("em", email);
            return await _collection.Find(filter).SingleOrDefaultAsync();
        }

        public async Task<ICollection<string>> GetBookedEmailsAsync()
        {
            var projection = Builders<UserEntity>.Projection.Include(x => x.Email);
            var usersEmails = await _collection.Find(x => true).Project<UserEntity>(projection).ToListAsync();
            return usersEmails.Where(x => !String.IsNullOrEmpty(x.Email)).Select(x => x.Email).ToList();
        }
    }
}
