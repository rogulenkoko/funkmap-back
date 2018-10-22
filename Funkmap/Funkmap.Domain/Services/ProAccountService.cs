using System.Threading.Tasks;
using Funkmap.Cqrs.Abstract;
using Funkmap.Domain.Abstract.Repositories;
using Funkmap.Domain.Models;
using Funkmap.Payments.Contracts.Events;

namespace Funkmap.Domain.Services
{
    public class ProAccountService : IEventHandler<ProAccountConfirmedEvent>
    {
        private readonly IEventBus _eventBus;
        private readonly IProAccountRepository _proAccountRepository;

        public ProAccountService(IEventBus eventBus, IProAccountRepository proAccountRepository)
        {
            _eventBus = eventBus;
            _proAccountRepository = proAccountRepository;
        }

        public void InitHandlers()
        {
            _eventBus.Subscribe<ProAccountConfirmedEvent>(Handle);
        }

        public async Task Handle(ProAccountConfirmedEvent @event)
        {
            var proAccount = new ProAccount
            {
                ExpireAt = @event.ExpireAt,
                UserLogin = @event.Login
            };

            await _proAccountRepository.CreateAsync(proAccount);
        }
    }
}
