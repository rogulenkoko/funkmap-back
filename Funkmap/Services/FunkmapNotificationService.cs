using System.Collections.Generic;
using System.Threading.Tasks;
using Funkmap.Common.Logger;
using Funkmap.Common.RedisMq;
using Funkmap.Contracts.Notifications;
using Funkmap.Models.Requests;
using Funkmap.Services.Abstract;
using ServiceStack.Messaging;

namespace Funkmap.Services
{
    public class FunkmapNotificationService : RedisMqProducer, IRedisMqConsumer, IFunkmapNotificationService
    {
        private readonly IMessageService _messageService;
        private readonly IFunkmapLogger<FunkmapNotificationService> _logger;
        private readonly IDependenciesController _dependenciesController;

        public FunkmapNotificationService(IMessageFactory redisMqFactory,
                                          IMessageService messageService,
                                          IDependenciesController dependenciesController,
                                          IFunkmapLogger<FunkmapNotificationService> logger) : base(redisMqFactory)
        {
            _messageService = messageService;
            _logger = logger;
            _dependenciesController = dependenciesController;
        }

        public void InitHandlers()
        {
            _messageService.RegisterHandler<InviteToBandBack>(request => OnBandInviteAnswered(request?.GetBody()));
        }

        private async Task OnBandInviteAnswered(InviteToBandBack request)
        {
            var inviteRequest = request.Notification as InviteToBandRequest;
            if (inviteRequest == null)
            {
                _logger.Info("Обратный запрос неопределен или не соответстует нужному типу. Ответ будет проигнорирован");
                return;
            }

            var updateRequest = new UpdateBandMembersRequest()
            {
                MusicianLogin = inviteRequest.InvitedMusicianLogin,
                BandLogin = inviteRequest.BandLogin
            };
            await _dependenciesController.CreateDependencies(updateRequest, request.Answer);
        }

        public void InviteMusicianToGroup(InviteToBandRequest request)
        {
            Publish<InviteToBandRequest>(request);
        }
    }
}
