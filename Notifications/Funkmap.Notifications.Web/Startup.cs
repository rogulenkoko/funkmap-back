using Autofac;
using Funkmap.Common.Core.Filters;
using Funkmap.Common.Core.Tools;
using Funkmap.Notifications.Contracts;
using Funkmap.Notifications.Contracts.Abstract;
using Funkmap.Notifications.Data;
using Funkmap.Notifications.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
            services.AddFunkmap(Configuration);
            
            services.AddCors();
            services.AddMvc(options =>
            {
                options.Filters.Add<ValidateRequestModelFilter>();
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

            app.UseFunkmap();
            
            app.UseMvc();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterNotificationModule();
            builder.RegisterNotificationDataModule(Configuration);
            builder.RegisterCommonModule(Configuration);

            builder.RegisterType<FunkmapNotificationService>().As<IFunkmapNotificationService>();
        }
    }
}