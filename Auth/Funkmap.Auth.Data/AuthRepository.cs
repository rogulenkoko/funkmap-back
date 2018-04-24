using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Funkmap.Auth.Data.Entities;
using Funkmap.Common.Abstract;
using Funkmap.Common.Data.Mongo;
using MongoDB.Driver;
using Autofac.Features.AttributeFilters;
using Funkmap.Auth.Data.Mappers;
using Funkmap.Auth.Domain.Abstract;
using Funkmap.Auth.Domain.Models;
using Funkmap.Common.Cqrs;
using Funkmap.Common.Tools;

namespace Funkmap.Auth.Data
{
    public class AuthRepository : RepositoryBase<UserEntity>, IAuthRepository
    {
        private readonly IFileStorage _fileStorage;

        public AuthRepository(IMongoCollection<UserEntity> collection,
                              [KeyFilter(AuthCollectionNameProvider.AuthStorageName)] IFileStorage fileStorage) : base(collection)
        {
            _fileStorage = fileStorage;
        }

        public async Task<User> GetAsync(string login)
        {
            await Task.Yield();
            var filter = Builders<UserEntity>.Filter.Eq(x => x.Login, login);
            var entity = await _collection.Find(filter).SingleOrDefaultAsync();
            return entity.ToUser();
        }

        public async Task<CommandResponse> TryCreateAsync(User user, string hashedPassword)
        {
            var entity = user.ToEntity(hashedPassword);

            if (String.IsNullOrWhiteSpace(user.Email))
            {
                return new CommandResponse(false) {Error = "Invalid user's email."};
            }

            if (String.IsNullOrWhiteSpace(user.Login))
            {
                return new CommandResponse(false) { Error = "Invalid user's login." };
            }

            if (String.IsNullOrWhiteSpace(hashedPassword))
            {
                return new CommandResponse(false) {Error = "Invalid user's password."};
            }

            try
            {
                await _collection.InsertOneAsync(entity);
                return new CommandResponse(true);
            }
            catch (Exception e)
            {
                return new CommandResponse(false) {Error = e.Message};
            }
           
        }

        public async Task<User> LoginAsync(string login, string hashedPassword)
        {
            var user = await _collection.Find(x=>(x.Login == login || x.Email == login) && x.Password == hashedPassword)
                .SingleOrDefaultAsync();
            return user.ToUser();
        }

        public async Task<byte[]> GetAvatarAsync(string login)
        {
            var projection = Builders<UserEntity>.Projection.Include(x => x.AvatarUrl).Include(x=>x.Login);
            var filter = Builders<UserEntity>.Filter.Eq(x => x.Login, login);
            var user = await _collection.Find(filter).Project<UserEntity>(projection).SingleOrDefaultAsync();

            if (String.IsNullOrEmpty(user?.AvatarUrl)) return null;
            
            try
            {
                var bytes = await _fileStorage.DownloadAsBytesAsync(user.AvatarUrl);
                return bytes;
            }
            catch (Exception)
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
            var update = Builders<UserEntity>.Update.Set(x => x.AvatarUrl, fullPath);
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

        public async Task UpdatePasswordAsync(string login, string password)
        {
            var update = Builders<UserEntity>.Update.Set(x => x.Password, password);
            await _collection.UpdateOneAsync(x => x.Login == login, update);
        }

        public async Task<List<string>> GetBookedEmailsAsync()
        {
            var projection = Builders<UserEntity>.Projection.Include(x => x.Email);
            var usersEmails = await _collection.Find(x => true).Project<UserEntity>(projection).ToListAsync();
            return usersEmails.Where(x => !String.IsNullOrEmpty(x.Email)).Select(x => x.Email).ToList();
        }
    }
}
