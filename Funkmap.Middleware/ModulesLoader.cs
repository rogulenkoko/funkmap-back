using System;
using System.Linq;
using Autofac;
using Funkmap.Common.Abstract;

namespace Funkmap.Middleware
{
    public class ModulesLoader
    {
        public void LoadAllModules(ContainerBuilder builder)
        {
            var modules = AppDomain.CurrentDomain.GetAssemblies()
                .Where(x => x.FullName.Contains("Funkmap.Module"))
                .SelectMany(s => s.GetTypes())
                .Where(x => x.GetInterfaces().Contains(typeof(IFunkmapModule)))
                .Distinct()
                .ToList();


            var instances = modules.Select(x => Activator.CreateInstance(x) as IFunkmapModule).ToList();
            foreach (var instance in instances)
            {
                instance.Register(builder);
            }

        }
    }
}
