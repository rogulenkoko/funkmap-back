using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using Funkmap.Module.Auth;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.OAuth;
using Owin;

namespace Funkmap
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            HttpConfiguration config = new HttpConfiguration();
            
            appBuilder.UseCors(CorsOptions.AllowAll);

            var containerBuilder = new ContainerBuilder();

            LoadAssemblies();
            RegisterModules(containerBuilder);

            var container = containerBuilder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            appBuilder.UseAutofacMiddleware(container);
            appBuilder.UseAutofacWebApi(config);

            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/api/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider = new FunkmapAuthProvider()
            };
            
            appBuilder.UseOAuthAuthorizationServer(OAuthServerOptions);
            appBuilder.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

            config.MapHttpAttributeRoutes();
            appBuilder.UseWebApi(config);
        }

        private void LoadAssemblies()
        {
            Assembly.Load("Funkmap.Module.Musician");
            Assembly.Load("Funkmap.Module.Search");
            Assembly.Load("Funkmap.Module.Auth");

        }


        private void RegisterModules(ContainerBuilder builder)
        {
            var loader = new ModulesLoader();
            loader.LoadAllModules(builder);
        }
    }

}
