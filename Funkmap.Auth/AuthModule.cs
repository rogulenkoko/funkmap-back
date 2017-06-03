using System;
using System.Reflection;
using Autofac;
using Autofac.Integration.WebApi;
using Funkmap.Common.Abstract;

namespace Funkmap.Module.Auth
{

    public class AuthModule : IModule
    {
        public void Register(ContainerBuilder builder)
        {
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            Console.WriteLine("Загружен модуль авторизации");
        }
    }
}
