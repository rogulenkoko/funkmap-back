using System;
using System.IO;
using System.Reflection;
using Autofac;
using Funkmap.Common.Core.Filters;
using Funkmap.Common.Core.Tools;
using Funkmap.Cqrs;
using Funkmap.Cqrs.Abstract;
using Funkmap.Feedback.Command;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace Funkmap.Feedback
{
    /// <summary>
    /// Run Feedback service
    /// </summary>
    public class FeedbackStartup
    {
        private IConfiguration Configuration { get; }
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configuration"><see cref="IConfiguration"/></param>
        public FeedbackStartup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddMvc(options =>
            {
                options.Filters.Add<ValidateRequestModelFilter>();
            });

            services.AddFunkmap(Configuration);
        }

        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Funkmap feedback API");
                c.RoutePrefix = string.Empty;
            });
            
            app.UseMvc();
        }

        /// <summary>
        /// Configure Autofac container
        /// </summary>
        /// <param name="builder"><see cref="ContainerBuilder"/></param>
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterFeedbackModule();
            builder.RegisterFeedbackCommandModule(Configuration);
            builder.RegisterCommonModule(Configuration);
        }
    }
}
