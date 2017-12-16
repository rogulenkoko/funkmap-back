using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
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
using Swashbuckle.Swagger;

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
            config.Routes.MapHttpRoute("Swagger UI", "", null, null, new RedirectHandler(SwaggerDocsConfig.DefaultRootUrlResolver, "swagger/ui/index"));


            appBuilder.UseWebApi(config);

            //SignalR

            var dependencyResolver = new AutofacDependencyResolver(container);
            var signalRConfig = new HubConfiguration()
            {
                EnableJavaScriptProxies = false,
                EnableDetailedErrors = false,
                Resolver = dependencyResolver
            };

            GlobalHost.DependencyResolver = dependencyResolver;
            appBuilder.MapSignalR("/signalr", signalRConfig);
        }

        private void LoadAssemblies()
        {
            Assembly.Load(typeof(Module.FunkmapModule).Assembly.FullName);
            Assembly.Load(typeof(AuthFunkmapModule).Assembly.FullName);
            Assembly.Load(typeof(Messenger.MessengerModule).Assembly.FullName);
            Assembly.Load(typeof(Notifications.NotificationsModule).Assembly.FullName);
            Assembly.Load(typeof(Statistics.StatisticsModule).Assembly.FullName);


            Assembly.Load(typeof(Common.Redis.Autofac.RedisModule).Assembly.FullName);
            Assembly.Load(typeof(LoggerModule).Assembly.FullName);
            Assembly.Load(typeof(Common.Notifications.NotificationToolModule).Assembly.FullName);


        }

        private void RegisterModules(ContainerBuilder builder)
        {
            var loader = new ModulesLoader();
            loader.LoadAllModules(builder);
        }

        private void InitializeSwagger(HttpConfiguration httpConfiguration)
        {
            // Swagger
            //http://.../swagger/ui/index

            httpConfiguration.EnableSwagger(swaggerConfig =>
            {
                swaggerConfig.SingleApiVersion("v1", "Funkmap");

                string executablePath = AppDomain.CurrentDomain.BaseDirectory;

                AppDomain.CurrentDomain.GetAssemblies()
                    .Where(x => x.FullName.Contains("Funkmap"))
                    .ToList().ForEach(assembly =>
                    {
                        string filePath = $"{executablePath}\\{assembly.GetName().Name}.XML";
                        if (!File.Exists(filePath)) return;

                        swaggerConfig.IncludeXmlComments(filePath);
                    });

                //x.ApiKey("Token")
                //    .Description("Bearer token")
                //    .Name("Authorization")
                //    .In("header");

                swaggerConfig.OperationFilter<AuthDocumentFilter>();


                //swaggerConfig.OAuth2("Auth")
                //    .AuthorizationUrl("/api/token")
                //    .TokenUrl("/api/token");

            }).EnableSwaggerUi(x => x.EnableApiKeySupport("Authorization", "header"));
        }


        public class AuthDocumentFilter : IOperationFilter
        {
            public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
            {
                // Determine if the operation has the Authorize attribute
                var authorizeAttributes = apiDescription.ActionDescriptor.GetCustomAttributes<System.Web.Http.AuthorizeAttribute>();

                if (!authorizeAttributes.Any())
                    return;

                // Initialize the operation.security property
                if (operation.security == null)
                    operation.security = new List<IDictionary<string, IEnumerable<string>>>();


                // Add the appropriate security definition to the operation
                var parameter = new Parameter
                {
                    description = "The authorization token",
                    @in = "header",
                    name = "Authorization",
                    required = true,
                    type = "string"
                };
                
                operation.parameters = new List<Parameter>() { parameter };
            }
        }
    }

}
