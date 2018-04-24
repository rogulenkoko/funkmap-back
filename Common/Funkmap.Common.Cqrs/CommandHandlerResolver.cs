using System;
using Autofac;
using Funkmap.Common.Cqrs.Abstract;

namespace Funkmap.Common.Cqrs
{
    public class CommandHandlerResolver : ICommandHandlerResolver
    {

        private readonly IComponentContext _componentContext;

        public CommandHandlerResolver(IComponentContext componentContext)
        {
            _componentContext = componentContext;
        }


        public TCommandHandler ResolveCommandHandler<TCommandHandler>() where TCommandHandler : class, ICommandHandler
        {
            if (!_componentContext.IsRegistered<TCommandHandler>())
            {
                throw new InvalidOperationException("Handler for this command is not registered");
            }

            return _componentContext.Resolve<TCommandHandler>();
        }
    }
}
