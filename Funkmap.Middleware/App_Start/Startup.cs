using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Cors;
using Autofac;
using Autofac.Integration.SignalR;
using Autofac.Integration.WebApi;
using Funkmap.Auth;
using Funkmap.Auth.Data;
using Funkmap.Common.Notifications;
using Funkmap.Common.Owin.Filters;
using Funkmap.Common.Tools;
using Funkmap.Data;
using Funkmap.Feedback;
using Funkmap.Feedback.Command;
using Funkmap.Logger;
using Funkmap.Logger.Autofac;
using Funkmap.Messenger;
using Funkmap.Messenger.Command;
using Funkmap.Messenger.Query;
using Funkmap.Middleware.Handlers;
using Funkmap.Module;
using Funkmap.Notifications;
using Funkmap.Notifications.Data;
using Metrics;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.OAuth;
using Owin;
using Owin.Metrics;
using Swagger.Net.Application;

namespace Funkmap.Middleware
{
    public partial class Startup
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
            containerBuilder.RegisterModule<LoggerModule>();


            var container = containerBuilder.Build();

            var logger = container.Resolve<IFunkmapLogger<FunkmapMiddleware>>();
            appBuilder.Use<FunkmapMiddleware>(logger);

            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            config.Filters.Add(new ValidateRequestModelAttribute());
           // config.MessageHandlers.Add(new LanguageDelegateHandler());

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

#if DEBUG
            //Metrics
            config.MessageHandlers.Add(new MetricsHandler());
            Metric.Config
                .WithAllCounters()
                .WithOwin(middleware => appBuilder.Use(middleware), metricsConfig => metricsConfig
                    .WithRequestMetricsConfig(c => c.WithAllOwinMetrics())
                    .WithMetricsEndpoint()
                );
#endif

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
            Assembly.Load(typeof(FunkmapModule).Assembly.FullName);
            Assembly.Load(typeof(FunkmapMongoModule).Assembly.FullName);


            Assembly.Load(typeof(AuthFunkmapModule).Assembly.FullName);
            Assembly.Load(typeof(AuthMongoModule).Assembly.FullName);

            Assembly.Load(typeof(MessengerModule).Assembly.FullName);
            Assembly.Load(typeof(MessengerCommandModule).Assembly.FullName);
            Assembly.Load(typeof(MessengerQueryModule).Assembly.FullName);

            Assembly.Load(typeof(NotificationsModule).Assembly.FullName);
            Assembly.Load(typeof(NotificationsMongoModule).Assembly.FullName);
            
            Assembly.Load(typeof(FeedbackModule).Assembly.FullName);
            Assembly.Load(typeof(FeedbackCommandModule).Assembly.FullName);
            Assembly.Load(typeof(NotificationToolModule).Assembly.FullName);
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
                swaggerConfig.SingleApiVersion("v1", "Bandmap");

                swaggerConfig.DescribeAllEnumsAsStrings();

                string executablePath = new Uri(Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase)).LocalPath;

                AppDomain.CurrentDomain.GetAssemblies()
                    .Where(x => x.FullName.Contains("Funkmap"))
                    .ToList().ForEach(assembly =>
                    {
                        string filePath = $"{executablePath}\\{assembly.GetName().Name}.XML";
                        if (!File.Exists(filePath)) return;

                        swaggerConfig.IncludeXmlComments(filePath);
                    });

                swaggerConfig.OAuth2("Bandmap").TokenUrl($"/api/token").Flow("password");
                swaggerConfig.OperationFilter<SwaggerConfig.AuthTokenOperationFilter>();

            }).EnableSwaggerUi(ui =>
            {
                ui.EnableOAuth2Support(String.Empty, String.Empty, String.Empty);
            });
        }
    }

}
