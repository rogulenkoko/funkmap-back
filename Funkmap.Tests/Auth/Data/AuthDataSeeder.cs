﻿using System;
using Funkmap.Auth.Data;
using Funkmap.Auth.Data.Entities;
using Funkmap.Common.Abstract;
using Funkmap.Module.Auth;
using Funkmap.Tests.Images;
using MongoDB.Driver;
using Moq;

namespace Funkmap.Tests.Auth.Data
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

            var fileStorage = new Mock<IFileStorage>();

            var repository = new AuthRepository(_database.GetCollection<UserEntity>(AuthCollectionNameProvider.UsersCollectionName), fileStorage.Object);
            var u1 = new UserEntity()
            {
                Login = "rogulenkoko",
                Name = "Кирилл Евгеньевич",
                Password = "1",
                Email = "rogulenkoko@gmail.com",
                LastVisitDateUtc = DateTime.UtcNow.AddMinutes(-20)
            };


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
