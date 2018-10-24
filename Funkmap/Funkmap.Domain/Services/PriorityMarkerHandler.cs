using System.Threading.Tasks;
using Funkmap.Cqrs.Abstract;
using Funkmap.Domain.Abstract.Repositories;
using Funkmap.Payments.Contracts.Events;

namespace Funkmap.Domain.Services
{
    public class PriorityMarkerHandler : IEventHandler<PriorityMarkerConfirmedEvent>
    {
        private readonly IEventBus _eventBus;
        private readonly IBaseCommandRepository _commandRepository;

        public PriorityMarkerHandler(IEventBus eventBus, IBaseCommandRepository commandRepository)
        {
            _eventBus = eventBus;
            _commandRepository = commandRepository;
        }

        public void InitHandlers()
        {
            _eventBus.Subscribe<PriorityMarkerConfirmedEvent>(Handle);
        }

        public async Task Handle(PriorityMarkerConfirmedEvent @event)
        {
            await _commandRepository.UpdatePriorityAsync(@event.ProfileLogin);
        }
    }
}
