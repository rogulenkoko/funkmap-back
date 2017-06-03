using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using Microsoft.Owin.Cors;
using Owin;

namespace Funkmap
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            // Configure Web API for self-host. 
            HttpConfiguration config = new HttpConfiguration();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            appBuilder.UseCors(CorsOptions.AllowAll);

            var containerBuilder = new ContainerBuilder();

            LoadAssemblies();
            RegisterModules(containerBuilder);

            var container = containerBuilder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            appBuilder.UseAutofacMiddleware(container);
            appBuilder.UseAutofacWebApi(config);


            appBuilder.UseWebApi(config);
        }

        private void LoadAssemblies()
        {
            Assembly.Load("Funkmap.Module.Musician");
            Assembly.Load("Funkmap.Module.Search");

        }


        private void RegisterModules(ContainerBuilder builder)
        {
            var loader = new ModulesLoader();
            loader.LoadAllModules(builder);
        }
    }

}
