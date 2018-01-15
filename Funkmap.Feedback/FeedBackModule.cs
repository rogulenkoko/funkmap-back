using System;
using Autofac;
using Funkmap.Common.Abstract;

namespace Funkmap.Feedback
{
    public class FeedBackModule : IFunkmapModule
    {
        public void Register(ContainerBuilder builder)
        {


            Console.WriteLine("Загружен модуль обратной связи");
        }
    }
}
