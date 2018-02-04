using System;
using System.Linq;
using Autofac;
using Funkmap.Common.Abstract;

namespace Funkmap.Common.Tools
{
    public class BoostrappersConfigurator
    {
        public void Configure(IContainer container)
        {
            var modules = AppDomain.CurrentDomain.GetAssemblies()
                .Where(x => x.FullName.Contains("Funkmap"))
                .SelectMany(s => s.GetTypes())
                .Where(x => x.GetInterfaces().Contains(typeof(IFunkmapBootstrapper)))
                .Distinct()
                .ToList();


            var instances = modules.Select(x => Activator.CreateInstance(x) as IFunkmapBootstrapper).ToList();
            foreach (var instance in instances)
            {
                instance.Configure(container);
            }

        }
    }
}
