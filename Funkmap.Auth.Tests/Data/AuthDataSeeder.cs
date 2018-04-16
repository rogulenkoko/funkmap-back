using System;
using System.Collections.Generic;
using Funkmap.Auth.Data;
using Funkmap.Auth.Data.Entities;
using MongoDB.Driver;

namespace Funkmap.Auth.Tests.Data
{
    public class AuthDataSeeder
    {
        private readonly IMongoDatabase _database;

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
            var collection = _database.GetCollection<UserEntity>(AuthCollectionNameProvider.UsersCollectionName);


            var users = new List<UserEntity>()
            {
                new UserEntity
                {
                    Login = "rogulenkoko",
                    Name = "Кирилл Евгеньевич",
                    Password = "1",
                    Email = "rogulenkoko@gmail.com",
                    LastVisitDateUtc = DateTime.UtcNow.AddMinutes(-20)
                },
                new UserEntity
                {
                    Login = "test",
                    Name = "Тест",
                    Password = "1",
                    Email = "test@mail.ru"
                },
                new UserEntity
                {
                    Login = "timosha",
                    Password = "123",
                    Email = "timoshka_kirov@mail.ru"
                }
            };

            collection.InsertManyAsync(users).GetAwaiter().GetResult();
        }
    }
}
