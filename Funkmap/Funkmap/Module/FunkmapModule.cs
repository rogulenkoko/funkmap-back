using System;
using Autofac;
using Funkmap.Cqrs.Abstract;
using Funkmap.Domain.Services;
using Funkmap.Domain.Services.Abstract;
using Funkmap.Tools;
using Funkmap.Tools.Abstract;

namespace Funkmap.Module
{
    public static class FunkmapModule
    {
        public static void RegisterFunkmapDomainModule(this ContainerBuilder builder)
        {
            builder.RegisterType<ParameterFactory>().As<IParameterFactory>();

            builder.RegisterType<FunkmapNotificationHandler>()
                .As<IEventHandler>()
                .SingleInstance()
                .OnActivated(x => x.Instance.InitHandlers())
                .AutoActivate();

            builder.RegisterType<BandUpdateService>().As<IBandUpdateService>();
            builder.RegisterType<AccessService>().As<IAccessService>();

            builder.RegisterType<ProAccountHandler>()
                .As<IEventHandler>()
                .SingleInstance()
                .OnActivated(x => x.Instance.InitHandlers())
                .AutoActivate();
            
            Console.WriteLine("Funkmap module has been loaded.");
        }
    }
}
