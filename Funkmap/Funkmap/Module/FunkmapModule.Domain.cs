using Autofac;
using Funkmap.Cqrs.Abstract;
using Funkmap.Domain.Services;
using Funkmap.Domain.Services.Abstract;
using Funkmap.Notifications.Contracts;
using Funkmap.Tools;
using Funkmap.Tools.Abstract;

namespace Funkmap.Module
{
    public partial class FunkmapModule 
    {
        private void RegisterDomainDependencies(ContainerBuilder builder)
        {
            builder.RegisterType<ParameterFactory>().As<IParameterFactory>();

            builder.RegisterType<FunkmapNotificationHandler>()
                .As<IEventHandler>()
                .SingleInstance()
                .OnActivated(x => x.Instance.InitHandlers())
                .AutoActivate();

            builder.RegisterType<FunkmapNotificationService>().As<IFunkmapNotificationService>();

            builder.RegisterType<BandUpdateService>().As<IBandUpdateService>();
            builder.RegisterType<AccessService>().As<IAccessService>();

            builder.RegisterType<ProAccountHandler>()
                .As<IEventHandler>()
                .SingleInstance()
                .OnActivated(x => x.Instance.InitHandlers())
                .AutoActivate();
        }
    }
}
