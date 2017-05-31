using System;
using Autofac;
using Funkmap.Common.Abstract;

namespace Funkmap.Module.Musician
{
    public class MusicianModule : IModule
    {
        public void Register(ContainerBuilder builder)
        {
            Console.WriteLine("Загружен модуль музыкантов");
        }
    }
}
