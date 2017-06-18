using System;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Cors;
using Autofac;
using Autofac.Integration.WebApi;
using Funkmap.Common.Filters;
using Funkmap.Common.Notification;
using Funkmap.Common.Notification.Abstract;
using Funkmap.Module.Auth;
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

            var container = containerBuilder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            config.Filters.Add(new ValidateRequestModelAttribute());

            appBuilder.UseAutofacMiddleware(container);
            appBuilder.UseAutofacWebApi(config);

            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/api/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider = new FunkmapAuthProvider(),
                RefreshTokenProvider = new FunkmapRefreshTokenProvider(),
                
                
            };
            
            appBuilder.UseOAuthAuthorizationServer(OAuthServerOptions);
            appBuilder.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
            
            config.MapHttpAttributeRoutes();
            appBuilder.UseWebApi(config);
        }

        private void LoadAssemblies()
        {
            Assembly.Load("Funkmap");
            Assembly.Load("Funkmap.Module.Auth");
            Assembly.Load("Funkmap.Messenger");
        }


        private void RegisterModules(ContainerBuilder builder)
        {
            var loader = new ModulesLoader();
            loader.LoadAllModules(builder);

            builder.RegisterType<EmailNotificationService>().As<INotificationService>();
        }
    }

}
