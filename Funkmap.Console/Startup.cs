using System;
using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using Funkmap.Common.Filters;
using Funkmap.Common.Logger;
using Funkmap.Common.Tools;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.OAuth;
using Owin;

namespace Funkmap.Console
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

            //containerBuilder.RegisterType<FunkmapAuthProvider>();



            var container = containerBuilder.Build();

            var logger = container.Resolve<IFunkmapLogger<FunkmapMiddleware>>();
            appBuilder.Use<FunkmapMiddleware>(logger);

            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            config.Filters.Add(new ValidateRequestModelAttribute());

            appBuilder.UseAutofacMiddleware(container);
            appBuilder.UseAutofacWebApi(config);

            //OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            //{
            //    AllowInsecureHttp = true,
            //    TokenEndpointPath = new PathString() new PathString("http://localhost:9001/api/token"),
            //    //AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
            //    Provider = new OAuthAuthorizationServerProvider()
            //    {
            //        OnValidateClientRedirectUri = 
            //    },
            //    //RefreshTokenProvider = new FunkmapRefreshTokenProvider(),
            //    //AuthorizeEndpointPath =
                
            //};

            //appBuilder.UseOAuthAuthorizationServer(OAuthServerOptions);
            appBuilder.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());


            config.MapHttpAttributeRoutes();
            appBuilder.UseWebApi(config);
        }

        private void LoadAssemblies()
        {
            Assembly.Load(typeof(Module.FunkmapModule).Assembly.FullName);
            Assembly.Load("Funkmap.Common.Modules");
        }


        private void RegisterModules(ContainerBuilder builder)
        {
            var loader = new ModulesLoader();
            loader.LoadAllModules(builder);
        }
    }
}
