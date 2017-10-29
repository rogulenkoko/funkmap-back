using Funkmap.Common.Logger;
using Funkmap.Common.Redis.Abstract;
using Funkmap.Common.Redis.Options;
using Funkmap.Models.Requests;
using Funkmap.Notifications.Contracts;
using Funkmap.Notifications.Contracts.Specific;
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
            var options = new MessageQueueOptions()
            {
                SpecificKey = NotificationType.BandInvite,
                SerializerOptions = new SerializerOptions() { HasAbstractMember = true }
            };
            _messageQueue.Subscribe<NotificationAnswer>(OnBandInviteAnswered, options);
        }

        private void OnBandInviteAnswered(NotificationAnswer request)
        {
            var inviteRequest = request.Notification as BandInviteNotification;
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

        public void NotifyBandInvite(BandInviteNotification notification)
        {
            _messageQueue.PublishAsync(notification);
        }
    }
}
