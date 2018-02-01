using Autofac;
using Autofac.Features.AttributeFilters;
using Funkmap.Common.Abstract;
using Funkmap.Common.Cqrs.Abstract;
using Funkmap.Concerts.Command.CommandHandlers;
using Funkmap.Concerts.Command.Commands;

namespace Funkmap.Concerts.Command
{
    public class ConcertsCommandModule : IFunkmapModule
    {
        public void Register(ContainerBuilder builder)
        {
            builder.RegisterType<UpdateAfficheCommandHandler>().As<ICommandHandler<UpdateAfficheCommand>>().WithAttributeFiltering();
            builder.RegisterType<CreateConcertCommandHandler>().As<ICommandHandler<CreateConcertCommand>>();
        }
    }
}
