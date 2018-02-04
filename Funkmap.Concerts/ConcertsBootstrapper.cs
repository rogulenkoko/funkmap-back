using Akka.Actor;
using Akka.DI.AutoFac;
using Akka.DI.Core;
using Autofac;
using Funkmap.Common.Abstract;
using Funkmap.Concerts.Actors;
using Funkmap.Concerts.Messages;

namespace Funkmap.Concerts
{
    public class ConcertsBootstrapper : IFunkmapBootstrapper
    {
        public void Configure(IContainer container)
        {
            var system = container.Resolve<ActorSystem>();
            IDependencyResolver resolver = new AutoFacDependencyResolver(container, system);

            var actor = system.ActorOf(resolver.Create<SchedulerActor>(), nameof(SchedulerActor));

            actor.Tell(new InitializeSchedulersMessage());
        }
    }
}
