using System;
using System.IO;
using System.Reflection;
using Autofac;
using Funkmap.Common.Core.Auth;
using Funkmap.Common.Core.Filters;
using Funkmap.Common.Settings;
using Funkmap.Cqrs;
using Funkmap.Cqrs.Abstract;
using Funkmap.Logger;
using Funkmap.Notifications.Contracts;
using Funkmap.Notifications.Contracts.Abstract;
using Funkmap.Notifications.Data;
using Funkmap.Notifications.Hubs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using NLog;
using NLog.Config;
using Swashbuckle.AspNetCore.Swagger;

namespace Funkmap.Notifications.Web
{
    public class Startup
    {
        private IConfiguration Configuration { get; }
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configuration"><see cref="IConfiguration"/></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var authOptions = new FunkmapJwtOptions(Configuration);
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateAudience = true,
                        ValidateIssuer = true,
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        ValidIssuer = authOptions.Issuer,
                        ValidAudience = authOptions.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(authOptions.Key))
                    };
                });
            
            services.AddCors();
            services.AddMvc(options =>
            {
                options.Filters.Add<ValidateRequestModelFilter>();
            });
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info { Title = "Funkmap feedback API", Version = "v1" });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name.Replace(".Web","")}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
                
                options.AddSecurityDefinition("Bearer", new OAuth2Scheme
                {
                    Flow = "password",
                    TokenUrl = authOptions.TokenUrl,
                });

                options.DocumentFilter<OAuthDocumentFilter>();
            });
            
            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseAuthentication();

            app.UseSignalR(routes =>
            {
                routes.MapHub<NotificationsHub>("/notifications");
            });
            
            app.UseCors(builder =>
            {
                builder.AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin();
            });

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Funkmap feedback API");
                c.RoutePrefix = string.Empty;
            });
            
            app.UseMvc();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterNotificationModule();
            builder.RegisterNotificationDataModule(Configuration);
            
            builder.RegisterType<InMemoryEventBus>().As<IEventBus>();
            builder.RegisterType<InMemoryCommandBus>().As<ICommandBus>();
            builder.RegisterType<FunkmapNotificationService>().As<IFunkmapNotificationService>();
            
            //builder.RegisterModule<LoggerModule>();
            
            builder.Register(container =>
            {
                var localPath = new Uri(Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase)).LocalPath;
                var email = Path.Combine(localPath, "NLogEmail.config");
                var file = Path.Combine(localPath, "NLog.config");

                var loggingType = (LoggingType)Enum.Parse(typeof(LoggingType), Configuration["Logging:Type"]);
                
                string fileName;
                switch (loggingType)
                {
                    case LoggingType.Empty:
                        fileName = "";
                        break;
                    case LoggingType.File:
                        fileName = file;
                        break;
                    case LoggingType.Email:
                        fileName = email;
                        break;
                    default:
                        fileName = file;
                        break;
                }
                
                if (!string.IsNullOrEmpty(fileName))
                    LogManager.Configuration = (LoggingConfiguration) new XmlLoggingConfiguration(fileName);
                return LogManager.GetCurrentClassLogger();
            }).SingleInstance().As<ILogger>();
            
            builder.RegisterGeneric(typeof (FunkmapLogger<>)).As(new Type[1]
            {
                typeof (IFunkmapLogger<>)
            });
        }
    }
}