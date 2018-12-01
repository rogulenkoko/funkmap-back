using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Cors;
using Autofac;
using Autofac.Integration.WebApi;
using Funkmap.Auth.Data;
using Funkmap.Common.Owin.Filters;
using Funkmap.Common.Tools;
using Funkmap.Logger.Autofac;
using Funkmap.Middleware;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.Jwt;
using Microsoft.Owin.Security.OAuth;
using Owin;
using Swagger.Net.Application;
using Thinktecture.IdentityModel.Tokens;

namespace Funkmap.Auth.Web
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

            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            config.Filters.Add(new ValidateRequestModelAttribute());
            config.MessageHandlers.Add(new LanguageDelegateHandler());

            appBuilder.UseAutofacMiddleware(container);
            appBuilder.UseAutofacWebApi(config);


            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                AccessTokenFormat = new FunkmapJwtFormat(),
                TokenEndpointPath = new PathString("/api/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider = container.Resolve<FunkmapAuthProvider>(),
                RefreshTokenProvider = new FunkmapRefreshTokenProvider()
            };

            appBuilder.UseOAuthAuthorizationServer(OAuthServerOptions);
            
            appBuilder.UseJwtBearerAuthentication(new JwtBearerAuthenticationOptions()
            {
                TokenValidationParameters = new System.IdentityModel.Tokens.TokenValidationParameters()
                {
                    IssuerSigningKey = new HmacSigningCredentials(FunkmapJwtOptions.Key).SigningKey,
                    ValidAudience = FunkmapJwtOptions.Audience,
                    ValidIssuer = FunkmapJwtOptions.Issuer,
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = true,
                    ValidateIssuer = true
                }
            });



            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute("Swagger UI", "", null, null, new RedirectHandler(SwaggerDocsConfig.DefaultRootUrlResolver, "swagger/ui/index"));

            appBuilder.UseWebApi(config);
        }

        private void LoadAssemblies()
        {            
            Assembly.Load(typeof(AuthFunkmapModule).Assembly.FullName);
            Assembly.Load(typeof(AuthMongoModule).Assembly.FullName);
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
