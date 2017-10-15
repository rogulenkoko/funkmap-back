using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Auth.Data;
using Funkmap.Auth.Data.Entities;
using Funkmap.Module.Auth;
using Funkmap.Tests.Images;
using MongoDB.Driver;

namespace Funkmap.Tests.Funkmap.Auth
{
    public class AuthDataSeeder
    {
        private IMongoDatabase _database;

        public AuthDataSeeder(IMongoDatabase database)
        {
            _database = database;
            
        }

        public void SeedData()
        {
            SeedUsers();
        }

        private void SeedUsers()
        {
            var repository = new AuthRepository(_database.GetCollection<UserEntity>(AuthCollectionNameProvider.UsersCollectionName));
            var u1 = new UserEntity()
            {
                Login = "rogulenkoko",
                Name = "Кирилл Евгеньевич",
                Password = "1",
                Email = "rogulenkoko@gmail.com",
                LastVisitDateUtc = DateTime.UtcNow.AddMinutes(-20)
            };
            u1.Avatar = ImageProvider.GetAvatar("avatar.jpg");


            var u2 = new UserEntity()
            {
                Login = "test",
                Name = "Тест",
                Password = "1",
                Email = "test@mail.ru"
            };

            var u3 = new UserEntity()
            {
                Login = "timosha",
                Password = "123",
                Email = "timoshka_kirov@mail.ru"
            };
            
            repository.CreateAsync(u1).Wait();
            repository.CreateAsync(u2).Wait();
            repository.CreateAsync(u3).Wait();
        }
    }
}
