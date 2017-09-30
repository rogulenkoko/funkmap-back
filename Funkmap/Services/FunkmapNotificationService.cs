using System.Linq;
using System.Threading.Tasks;
using Funkmap.Common.Logger;
using Funkmap.Common.RedisMq;
using Funkmap.Contracts.Notifications;
using Funkmap.Data.Entities;
using Funkmap.Data.Repositories.Abstract;
using Funkmap.Services.Abstract;
using ServiceStack.Messaging;

namespace Funkmap.Services
{
    public class FunkmapNotificationService : RedisMqProducer, IRedisMqConsumer, IFunkmapNotificationService
    {
        private readonly IMessageService _messageService;
        private readonly IBaseRepository _repository;
        private readonly IFunkmapLogger<FunkmapNotificationService> _logger;
        public FunkmapNotificationService(IMessageFactory redisMqFactory, 
                                          IMessageService messageService,
                                          IBaseRepository repository,
                                          IFunkmapLogger<FunkmapNotificationService> logger) : base(redisMqFactory)
        {
            _messageService = messageService;
            _repository = repository;
            _logger = logger;
        }

        public void InitHandlers()
        {
            _messageService.RegisterHandler<InviteToBandBack>(request=> OnBandInviteAnswered(request?.GetBody()));
        }

        private async Task OnBandInviteAnswered(InviteToBandBack request)
        {
            var inviteRequest = request.Notification as InviteToBandRequest;
            if (inviteRequest == null)
            {
                _logger.Info("Обратный запрос неопределен или не соответстует нужному типу. Ответ будет проигнорирован");
                return;
            }

            var entity = await _repository.GetSpecificNavigationAsync(new[] { inviteRequest.BandLogin });
            var band = entity.FirstOrDefault() as BandEntity;
            if (band == null) return;

            if (request.Answer)
            {
                if (band.MusicianLogins.Contains(inviteRequest.InvitedMusicianLogin)) return;
                band.MusicianLogins.Add(inviteRequest.InvitedMusicianLogin);
                band.InvitedMusicians.Remove(inviteRequest.InvitedMusicianLogin);
            }
            else
            {
                band.InvitedMusicians.Remove(inviteRequest.InvitedMusicianLogin);
            }

            await _repository.UpdateAsync(band);
        }

        public void InviteMusicianToGroup(InviteToBandRequest request)
        {
            Publish<InviteToBandRequest>(request);
        }
    }
}
