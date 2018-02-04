using Akka.Actor;
using Funkmap.Common.Cqrs.Abstract;
using Funkmap.Concerts.Command.Commands;
using Funkmap.Concerts.Messages;

namespace Funkmap.Concerts.Actors
{
    public class ActivationActor : ICanTell
    {
        private readonly ICommandBus _commandBus;

        public ActivationActor(ICommandBus commandBus)
        {
            _commandBus = commandBus;
        }

        public void Tell(object message, IActorRef sender)
        {
            var activationMessage = message as ActivationMessage;

            if (activationMessage == null) return;

            ActivateConcert(activationMessage);
        }

        private void ActivateConcert(ActivationMessage message)
        {
            _commandBus.Execute(new UpdateActivityCommand()
            {
                ConcertId = message.ConcertId,
                IsActive = true
            });

        }
    }
}
