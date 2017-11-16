using System;
using System.Reflection;
using Autofac;
using Autofac.Integration.WebApi;
using Funkmap.Common.Abstract;
using Funkmap.Statistics.Data.Repositories;
using Funkmap.Statistics.Data.Repositories.Abstract;
using Funkmap.Statistics.Data.Services;
using Funkmap.Statistics.Services;

namespace Funkmap.Statistics
{
    public partial class StatisticsModule : IFunkmapModule
    {
        public void Register(ContainerBuilder builder)
        {
            RegisterMongo(builder);

            builder.RegisterType<EntityTypeStatisticsRepository>().As<IProfileStatisticsRepository>().As<IStatisticsRepository>();
            builder.RegisterType<CityStatisticsRepository>().As<IProfileStatisticsRepository>().As<IStatisticsRepository>();
            builder.RegisterType<TopEntityStatisticsRepository>().As<IProfileStatisticsRepository>().As<IStatisticsRepository>();

            builder.RegisterType<TopStylesStatisticsRepository>().As<IMusicianStatisticsRepository>().As<IStatisticsRepository>();
            builder.RegisterType<InstrumentStatisticsRepository>().As<IMusicianStatisticsRepository>().As<IStatisticsRepository>();
            builder.RegisterType<SexStatisticsRepository>().As<IMusicianStatisticsRepository>().As<IStatisticsRepository>();
            builder.RegisterType<InBandStatisticsRepository>().As<IMusicianStatisticsRepository>().As<IStatisticsRepository>();

            builder.RegisterType<BaseStatisticsRepository>().As<IBaseStatisticsRepository>();
            builder.RegisterType<StatisticsBuilder>().As<IStatisticsBuilder>();
            builder.RegisterType<StatisticsMerger>().As<IStatisticsMerger>();
            builder.RegisterType<CitiesInfoProvider>().As<ICitiesInfoProvider>();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            Console.WriteLine("Загружен модуль статистик");
        }
    }
}
