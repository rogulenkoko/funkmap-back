
using System;
using Autofac;
using Funkmap.Common.Abstract;

namespace Funkmap.Concerts
{
    public class ConcertsModule : IFunkmapModule
    {
        public void Register(ContainerBuilder builder)
        {
            

            Console.WriteLine("Загружен модуль концертов");
        }
    }
}
