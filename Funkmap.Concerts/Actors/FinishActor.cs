using Akka.Actor;
using Funkmap.Common.Cqrs.Abstract;
using Funkmap.Concerts.Command.Commands;
using Funkmap.Concerts.Messages;

namespace Funkmap.Concerts.Actors
{
    public class FinishActor : ICanTell
    {
        private readonly ICommandBus _commandBus;

        public FinishActor(ICommandBus commandBus)
        {
            _commandBus = commandBus;
        }

        public void Tell(object message, IActorRef sender)
        {
            var command = message as FinishConcertMessage;

            if(command == null) return;

            FinishConcert(command);
        }

        private void FinishConcert(FinishConcertMessage message)
        {
            _commandBus.Execute(new FinishConcertCommand(message.ConcertId));
        }
    }
}
