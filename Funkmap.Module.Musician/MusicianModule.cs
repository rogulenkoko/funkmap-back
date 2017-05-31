using System;
using System.Reflection;
using Autofac;
using Autofac.Integration.WebApi;
using Funkmap.Common.Abstract;
using Funkmap.Module.Musician.Abstract;
using Funkmap.Module.Musician.Data;

namespace Funkmap.Module.Musician
{
    public class MusicianModule : IModule
    {
        public void Register(ContainerBuilder builder)
        {
            builder.RegisterType<MusicianRepository>().As<IMusicianRepository>().InstancePerLifetimeScope();
            builder.RegisterType<MusicianContext>().InstancePerLifetimeScope();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            Console.WriteLine("Загружен модуль музыкантов");
        }
    }
}
