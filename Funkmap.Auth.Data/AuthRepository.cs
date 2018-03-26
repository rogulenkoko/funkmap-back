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
using Funkmap.Common.Tools;

namespace Funkmap.Auth.Data
{
    public class AuthRepository : LoginRepository<UserEntity>, IAuthRepository
    {
        private readonly IFileStorage _fileStorage;

        public AuthRepository(IMongoCollection<UserEntity> collection,
                              [KeyFilter(AuthCollectionNameProvider.AuthStorageName)] IFileStorage fileStorage) : base(collection)
        {
            _fileStorage = fileStorage;
        }

        public async Task<UserEntity> LoginAsync(string login, string password)
        {
            var user = await _collection.Find(x=>(x.Login == login || x.Email == login) && x.Password == password)
                .SingleOrDefaultAsync();
            return user;
        }

        public async Task<bool> CheckIfExistAsync(string login)
        {
            var projection = Builders<UserEntity>.Projection.Include(x => x.Login);
            var userId = await _collection.Find(x => x.Login == login || x.Email == login).Project(projection).SingleOrDefaultAsync();
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
            String fullPath;
            if (image == null || image.Length == 0)
            {
                fullPath = String.Empty;
            }
            else
            {
                var minified = FunkmapImageProcessor.MinifyImage(image, 200);

                var date = DateTime.UtcNow.ToString("yyyyMMddhhmmss");
                var filename = $"{date}avatar_{login}.png";
                fullPath = await _fileStorage.UploadFromBytesAsync(filename, minified);
            }

           

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

        public async Task UpdateLocaleAsync(string login, string locale)
        {
            var update = Builders<UserEntity>.Update.Set(x => x.Locale, locale);
            await _collection.UpdateOneAsync(x => x.Login == login, update);
        }

        public async Task<DateTime?> GetLastVisitDateAsync(string login)
        {
            var dateProjection = Builders<UserEntity>.Projection.Include(x => x.LastVisitDateUtc);
            var user = await _collection.Find(x => x.Login == login)
                .Project<UserEntity>(dateProjection)
                .SingleOrDefaultAsync();
            return user?.LastVisitDateUtc;
        }

        public override async Task UpdateAsync(UserEntity entity)
        {
            var filter = Builders<UserEntity>.Filter.Eq(x => x.Login, entity.Login);
            await _collection.ReplaceOneAsync(filter, entity);
        }

        public async Task<List<string>> GetBookedEmailsAsync()
        {
            var projection = Builders<UserEntity>.Projection.Include(x => x.Email);
            var usersEmails = await _collection.Find(x => true).Project<UserEntity>(projection).ToListAsync();
            return usersEmails.Where(x => !String.IsNullOrEmpty(x.Email)).Select(x => x.Email).ToList();
        }

        public async Task<UserEntity> GetUserByEmailOrLoginAsync(string emailOrLogin)
        {
            var user = await _collection.Find(x => x.Login == emailOrLogin || x.Email == emailOrLogin).SingleOrDefaultAsync();

            return user;
        }
    }
}
