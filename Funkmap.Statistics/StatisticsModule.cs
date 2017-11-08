using System;
using System.Reflection;
using Autofac;
using Autofac.Integration.WebApi;
using Funkmap.Common.Abstract;

namespace Funkmap.Statistics
{
    public class StatisticsModule : IFunkmapModule
    {
        public void Register(ContainerBuilder builder)
        {
            //builder.RegisterType<>()

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            Console.WriteLine("Загружен модуль статистик");
        }
    }
}
