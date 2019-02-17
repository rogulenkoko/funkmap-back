using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Funkmap.Common.Core.Filters;
using Funkmap.Common.Core.Tools;
using Funkmap.Data;
using Funkmap.Feedback;
using Funkmap.Feedback.Command;
using Funkmap.Messenger;
using Funkmap.Module;
using Funkmap.Notifications;
using Funkmap.Notifications.Contracts;
using Funkmap.Notifications.Contracts.Abstract;
using Funkmap.Notifications.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace Funkmap.Web
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
            var assemblies = new[]
            {
                "Funkmap.Feedback",
                "Funkmap",
                "Funkmap.Messenger",
                "Funkmap.Notifications"
            };

            services.AddFunkmap(Configuration, assemblies);

            services.AddCors();
            services.AddMvc(options => { options.Filters.Add<ValidateRequestModelFilter>(); });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

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
            builder.RegisterFunkmapDomainModule();
            builder.RegisterFunkmapDataModule(Configuration);
            builder.RegisterCommonModule(Configuration);
            builder.RegisterFeedbackCommandModule(Configuration);
            builder.RegisterFeedbackModule();
            builder.RegisterNotificationDataModule(Configuration);
            builder.RegisterNotificationModule();
            builder.RegisterMessengerDomainModule();

            builder.RegisterType<FunkmapNotificationService>().As<IFunkmapNotificationService>();
        }
    }
}