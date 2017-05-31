using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Funkmap.Common.Abstract;

namespace Funkmap
{
    public class ModulesLoader
    {
        public void LoadAllModules(ContainerBuilder builder)
        {

            

            var modules = AppDomain.CurrentDomain.GetAssemblies()
                .Where(x => x.FullName.Contains("Funkmap.Module"))
                .SelectMany(s => s.GetTypes())
                .Where(x => x.GetInterfaces().Contains(typeof(IModule)))
                .Distinct()
                .ToList();


            var instances = modules.Select(x => Activator.CreateInstance(x) as IModule).ToList();
            foreach (var instance in instances)
            {
                instance.Register(builder);
            }

        }
    }
}
