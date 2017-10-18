using Funkmap.Common.Logger;
using Funkmap.Common.Redis.Abstract;
using Funkmap.Contracts.Notifications;
using Funkmap.Models.Requests;
using Funkmap.Services.Abstract;

namespace Funkmap.Services
{
    public class FunkmapNotificationService : IFunkmapNotificationService
    {
        private readonly IMessageQueue _messageQueue;
        private readonly IFunkmapLogger<FunkmapNotificationService> _logger;
        private readonly IDependenciesController _dependenciesController;

        public FunkmapNotificationService(IMessageQueue messageQueue,
                                          IDependenciesController dependenciesController,
                                          IFunkmapLogger<FunkmapNotificationService> logger)
        {
            _messageQueue = messageQueue;
            _logger = logger;
            _dependenciesController = dependenciesController;
        }

        public void InitHandlers()
        {
            _messageQueue.Subscribe<InviteToBandBack>(OnBandInviteAnswered);
        }

        private void OnBandInviteAnswered(InviteToBandBack request)
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
            _dependenciesController.CreateDependenciesAsync(updateRequest, request.Answer);
        }

        public void InviteMusicianToGroup(InviteToBandRequest request)
        {
            _messageQueue.PublishAsync(request);
        }
    }
}
