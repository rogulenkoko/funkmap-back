using System;
using System.Reflection;
using Autofac;
using Autofac.Integration.WebApi;
using Funkmap.Common.Abstract;
using Funkmap.Statistics.Data.Repositories;
using Funkmap.Statistics.Data.Repositories.Abstract;

namespace Funkmap.Statistics
{
    public class StatisticsModule : IFunkmapModule
    {
        public void Register(ContainerBuilder builder)
        {
            builder.RegisterType<EntityTypeStatisticsRepository>().As<IProfileStatisticsRepository>().As<IStatisticsRepository>();
            //builder.RegisterType<CityStatisticsRepository>().As<IProfileStatisticsRepository>().As<IStatisticsRepository>();
            builder.RegisterType<TopEntityStatisticsRepository>().As<IProfileStatisticsRepository>().As<IStatisticsRepository>();

            builder.RegisterType<TopEntityStatisticsRepository>().As<IMusicianStatisticsRepository>().As<IStatisticsRepository>();
            builder.RegisterType<TopStylesStatisticsRepository>().As<IMusicianStatisticsRepository>().As<IStatisticsRepository>();
            builder.RegisterType<InstrumentStatisticsRepository>().As<IMusicianStatisticsRepository>().As<IStatisticsRepository>();
            builder.RegisterType<SexStatisticsRepository>().As<IMusicianStatisticsRepository>().As<IStatisticsRepository>();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            Console.WriteLine("Загружен модуль статистик");
        }
    }
}
