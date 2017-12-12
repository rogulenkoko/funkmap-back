using System;
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
using Swashbuckle.Application;

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
            InitializeSwagger(config);

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


            var dependencyResolver = new AutofacDependencyResolver(container);

            signalRConfig.Resolver = dependencyResolver;
            GlobalHost.DependencyResolver = dependencyResolver;

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
            //Assembly.Load("Funkmap.Statistics");
        }

        private void RegisterModules(ContainerBuilder builder)
        {
            var loader = new ModulesLoader();
            loader.LoadAllModules(builder);
        }

        private void InitializeSwagger(HttpConfiguration httpConfiguration)
        {
            // Swagger
            //../swagger/ui/index
            httpConfiguration.EnableSwagger(x =>
            {
                x.SingleApiVersion("v1", "Funkmap");

                SwaggerConfig.SetCommentsPath(x);

                //x.ApiKey("Token")
                //    .Description("Bearer token")
                //    .Name("Authorization")
                //    .In("header");

                //x.DocumentFilter<AuthTokenDocumentFilter>();

            }).EnableSwaggerUi(x => x.EnableApiKeySupport("Authorization", "header"));
        }
    }

}
