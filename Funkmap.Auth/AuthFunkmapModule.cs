using System;
using System.Reflection;
using Autofac;
using Autofac.Integration.WebApi;
using Funkmap.Auth.Data;
using Funkmap.Auth.Data.Abstract;
using Funkmap.Common.Abstract;

namespace Funkmap.Module.Auth
{

    public class AuthFunkmapModule : IFunkmapModule
    {
        public void Register(ContainerBuilder builder)
        {
            builder.RegisterType<AuthRepository>().As<IAuthRepository>();
            builder.RegisterType<AuthContext>();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            Console.WriteLine("Загружен модуль авторизации");
        }
    }
}
