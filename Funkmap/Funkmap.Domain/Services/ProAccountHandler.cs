using System.Threading.Tasks;
using Funkmap.Cqrs.Abstract;
using Funkmap.Domain.Abstract.Repositories;
using Funkmap.Domain.Models;
using Funkmap.Domain.Notifications;
using Funkmap.Notifications.Contracts;
using Funkmap.Payments.Contracts.Events;

namespace Funkmap.Domain.Services
{
    public class ProAccountHandler : IEventHandler<ProAccountConfirmedEvent>
    {
        private readonly IEventBus _eventBus;
        private readonly IFunkmapNotificationService _notificationService;
        private readonly IProAccountRepository _proAccountRepository;

        public ProAccountHandler(IEventBus eventBus,
                                 IFunkmapNotificationService notificationService,
                                 IProAccountRepository proAccountRepository)
        {
            _eventBus = eventBus;
            _notificationService = notificationService;
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
                ExpireAt = @event.ExpireAtUtc,
                UserLogin = @event.Login
            };

            await _proAccountRepository.CreateAsync(proAccount);

            var notification = new ProAccountNotification
            {
                Login = @event.Login
            };
            await _notificationService.NotifyAsync(notification, @event.Login, FunkmapConstants.FunkmapLogin);
        }
    }
}
