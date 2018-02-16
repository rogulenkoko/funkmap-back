using System;
using System.Threading.Tasks;
using Funkmap.Common.Cqrs.Abstract;

namespace Funkmap.Common.Cqrs
{
    public class InMemoryCommandBus : ICommandBus
    {
        private readonly ICommandHandlerResolver _commandHandlerResolver;


        public InMemoryCommandBus(ICommandHandlerResolver commandHandlerResolver)
        {
            _commandHandlerResolver = commandHandlerResolver;
        }

        public virtual async Task ExecuteEnvelope<TCommand>(Envelope<TCommand> command) where TCommand : class
        {
            if (command == null)
            {
                throw new ArgumentException("command is null");
            }

            await Execute(command.Body);
        }

        public async Task Execute<TCommand>(TCommand commandBody) where TCommand : class
        {
            ICommandHandler<TCommand> handler = _commandHandlerResolver.ResolveCommandHandler<ICommandHandler<TCommand>>();

            await Task.Run(() =>
            {
                handler.Execute(commandBody);
            }).ConfigureAwait(false);
        }
    }
}
