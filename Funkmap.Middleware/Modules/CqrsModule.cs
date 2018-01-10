using Autofac;
using Funkmap.Common.Abstract;
using Funkmap.Common.Cqrs;
using Funkmap.Common.Cqrs.Abstract;
using Funkmap.Common.Redis;

namespace Funkmap.Middleware.Modules
{
    public class CqrsModule : IFunkmapModule
    {
        public void Register(ContainerBuilder builder)
        {
            builder.RegisterType<NewtonSerializer>().As<ISerializer>();
            builder.RegisterType<InMemoryEventBus>().As<IEventBus>().SingleInstance();

            builder.RegisterType<InMemoryCommandBus>().As<ICommandBus>();
            builder.RegisterType<CommandHandlerResolver>().As<ICommandHandlerResolver>();
        }
    }
}
