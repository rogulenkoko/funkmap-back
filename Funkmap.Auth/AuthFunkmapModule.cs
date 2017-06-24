using System;
using System.Collections.Generic;
using System.Reflection;
using Autofac;
using Autofac.Integration.WebApi;
using Funkmap.Auth.Data;
using Funkmap.Auth.Data.Abstract;
using Funkmap.Auth.Data.Entities;
using Funkmap.Common.Abstract;
using MongoDB.Driver;

namespace Funkmap.Module.Auth
{

    public class AuthFunkmapModule : IFunkmapModule
    {
        public void Register(ContainerBuilder builder)
        {
            var loginBaseIndexModel = new CreateIndexModel<UserEntity>(Builders<UserEntity>.IndexKeys.Ascending(x => x.Login), new CreateIndexOptions() { Unique = true });

            builder.Register(container => container.Resolve<IMongoDatabase>().GetCollection<UserEntity>(AuthCollectionNameProvider.UsersCollectionName))
                .OnActivating(async collection => await collection.Instance.Indexes
                    .CreateManyAsync(new List<CreateIndexModel<UserEntity>>() { loginBaseIndexModel}))
                .As<IMongoCollection<UserEntity>>();

            builder.RegisterType<AuthRepository>().As<IAuthRepository>();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            Console.WriteLine("Загружен модуль авторизации");
        }
    }
}
