using System;
using System.Reflection;
using Autofac;
using Autofac.Integration.WebApi;
using Funkmap.Common.Abstract;
using Funkmap.Common.Abstract.Search;
using Funkmap.Module.Musician.Services;
using Funkmap.Musician.Data;
using Funkmap.Musician.Data.Abstract;

namespace Funkmap.Module.Musician
{
    public class MusicianModule : IModule
    {
        public void Register(ContainerBuilder builder)
        {
            builder.RegisterType<MusicianRepository>().As<IMusicianRepository>();
            builder.RegisterType<MusicianContext>();
            builder.RegisterType<MusicianSearchService>().As<ISearchService>();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            Console.WriteLine("Загружен модуль музыкантов");
        }
    }
}
