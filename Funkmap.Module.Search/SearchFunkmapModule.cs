using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Integration.WebApi;
using Funkmap.Common.Abstract;

namespace Funkmap.Module.Search
{
    public class SearchFunkmapModule : IFunkmapModule
    {
        public void Register(ContainerBuilder builder)
        {
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            Console.WriteLine("Загружен модуль поиска");
        }
    }
}
