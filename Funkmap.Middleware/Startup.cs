using System;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Cors;
using Autofac;
using Autofac.Integration.SignalR;
using Autofac.Integration.WebApi;
using Funkmap.Common.Filters;
using Funkmap.Common.Logger;
using Funkmap.Common.Tools;
using Funkmap.Module.Auth;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.OAuth;
using Owin;

namespace Funkmap.Middleware
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            HttpConfiguration config = new HttpConfiguration();


            appBuilder.UseCors(CorsOptions.AllowAll);
            
            config.EnableCors(new EnableCorsAttribute("*", "*", "*"));

            var containerBuilder = new ContainerBuilder();


            LoadAssemblies();
            RegisterModules(containerBuilder);

            containerBuilder.RegisterType<FunkmapAuthProvider>();
            
            

            var container = containerBuilder.Build();

            var logger = container.Resolve<IFunkmapLogger<FunkmapMiddleware>>();
            appBuilder.Use<FunkmapMiddleware>(logger);

            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            config.Filters.Add(new ValidateRequestModelAttribute());

            appBuilder.UseAutofacMiddleware(container);
            appBuilder.UseAutofacWebApi(config);

            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/api/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider = container.Resolve<FunkmapAuthProvider>(),
                RefreshTokenProvider = new FunkmapRefreshTokenProvider()
            };
            
            appBuilder.UseOAuthAuthorizationServer(OAuthServerOptions);
            appBuilder.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

            

            config.MapHttpAttributeRoutes();
            appBuilder.UseWebApi(config);

            var signalRConfig = new HubConfiguration();
            signalRConfig.Resolver = new AutofacDependencyResolver(container);
            appBuilder.MapSignalR("/signalr", signalRConfig);
        }

        private void LoadAssemblies()
        {
            //todo
            Assembly.Load("Funkmap");
            Assembly.Load("Funkmap.Module.Auth");
            Assembly.Load("Funkmap.Messenger"); 
            Assembly.Load("Funkmap.Notifications");
            Assembly.Load("Funkmap.Common.Redis.Autofac");
        }

        private void RegisterModules(ContainerBuilder builder)
        {
            var loader = new ModulesLoader();
            loader.LoadAllModules(builder);
        }
    }

}
