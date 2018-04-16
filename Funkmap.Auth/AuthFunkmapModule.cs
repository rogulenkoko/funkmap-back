using System;
using System.Net.Http;
using System.Reflection;
using Autofac;
using Autofac.Integration.WebApi;
using Funkmap.Common.Abstract;
using Funkmap.Module.Auth.Abstract;
using Funkmap.Module.Auth.Services;

namespace Funkmap.Module.Auth
{
    public class AuthFunkmapModule : IFunkmapModule
    {
        public void Register(ContainerBuilder builder)
        {
            builder.RegisterType<ConfirmationCodeGenerator>().As<IConfirmationCodeGenerator>();
            builder.RegisterType<RegistrationContextManager>().As<IRegistrationContextManager>().As<IRestoreContextManager>().SingleInstance();
            
            builder.RegisterType<HttpClient>().SingleInstance().OnRelease(x => x.Dispose());
            builder.RegisterType<ClientSecretProvider>().As<IClientSecretProvider>().SingleInstance();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            Console.WriteLine("Загружен модуль авторизации");
        }
    }
}
