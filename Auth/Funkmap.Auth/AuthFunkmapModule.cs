using System;
using System.Net.Http;
using System.Reflection;
using Autofac;
using Autofac.Integration.WebApi;
using Funkmap.Auth.Abstract;
using Funkmap.Auth.Services;
using Funkmap.Common.Abstract;

namespace Funkmap.Auth
{
    /// <summary>
    /// IoC module for authorization domain module
    /// </summary>
    public class AuthFunkmapModule : IFunkmapModule
    {
        /// <inheritdoc cref="IFunkmapModule.Register"/>
        public void Register(ContainerBuilder builder)
        {
            builder.RegisterType<ConfirmationCodeGenerator>().As<IConfirmationCodeGenerator>().SingleInstance();
            builder.RegisterType<RegistrationContextManager>().As<IRegistrationContextManager>().As<IRestoreContextManager>().SingleInstance();
            
            builder.RegisterType<HttpClient>().SingleInstance().OnRelease(x => x.Dispose());
            builder.RegisterType<ClientSecretProvider>().As<IClientSecretProvider>().SingleInstance();

            builder.RegisterType<SocialUserFacade>(); 
            builder.RegisterType<FacebookUserService>().As<ISocialUserService>();
            builder.RegisterType<GoogleUserService>().As<ISocialUserService>();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            Console.WriteLine("Authorization module has been loaded.");
        }
    }
}
