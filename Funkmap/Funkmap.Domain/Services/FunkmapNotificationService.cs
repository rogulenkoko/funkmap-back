using System.Collections.Generic;
using System.Threading.Tasks;
using Funkmap.Common.Cqrs;
using Funkmap.Common.Cqrs.Abstract;
using Funkmap.Common.Logger;
using Funkmap.Domain.Abstract.Repositories;
using Funkmap.Domain.Models;
using Funkmap.Domain.Parameters;
using Funkmap.Domain.Services.Abstract;
using Funkmap.Notifications.Contracts;
using Funkmap.Notifications.Contracts.Events;
using Funkmap.Notifications.Contracts.Specific.BandInvite;

namespace Funkmap.Domain.Services
{
    public class FunkmapNotificationService : IFunkmapNotificationService
    {
        private readonly IEventBus _eventBus;
        private readonly IFunkmapLogger<FunkmapNotificationService> _logger;
        private readonly IBaseCommandRepository _commandRepository;
        private readonly IBaseQueryRepository _queryRepository;

        public FunkmapNotificationService(IBaseCommandRepository commandRepository,
                                          IBaseQueryRepository queryRepository,
                                          IEventBus eventBus,
                                          IFunkmapLogger<FunkmapNotificationService> logger)
        {
            _eventBus = eventBus;
            _commandRepository = commandRepository;
            _queryRepository = queryRepository;
            _logger = logger;
        }

        public void InitHandlers()
        {
            var options = new MessageQueueOptions
            {
                SpecificKey = NotificationType.BandInvite,
                SerializerOptions = new SerializerOptions { HasAbstractMember = true }
            };

            _eventBus.Subscribe<NotificationAnswer>(Handle, options);
        }

        public async Task Handle(NotificationAnswer answer)
        {
            var inviteRequest = answer.Notification as BandInviteNotification;
            
            if (inviteRequest == null)
            {
                _logger.Info("Response is null or has invalid type. Answer will be ignored.");
                return;
            }

            var band = await _queryRepository.GetAsync<Band>(inviteRequest.BandLogin);

            if (band.InvitedMusicians == null) band.InvitedMusicians = new List<string>();

            Band bandUpdate;

            if (answer.Answer)
            {
                if (band.Musicians == null) band.Musicians = new List<string>();

                band.Musicians.Add(inviteRequest.InvitedMusicianLogin);
                band.InvitedMusicians.Remove(inviteRequest.InvitedMusicianLogin);

                bandUpdate = new Band
                {
                    Login = band.Login,
                    Musicians = band.Musicians,
                    InvitedMusicians = band.InvitedMusicians
                };
            }
            else
            {
                band.InvitedMusicians.Remove(inviteRequest.InvitedMusicianLogin);
                bandUpdate = new Band
                {
                    Login = band.Login,
                    InvitedMusicians = band.InvitedMusicians
                };
            }

            CommandParameter<Profile> updateParameter = new CommandParameter<Profile>()
            {
                UserLogin = inviteRequest.SenderLogin,
                Parameter = bandUpdate
            };

            var updateResult = await _commandRepository.UpdateAsync(updateParameter);

            if (!updateResult.Success)
            {
                return;
            }

            var confirmation = new BandInviteConfirmationNotification
            {
                RecieverLogin = inviteRequest.SenderLogin,
                SenderLogin = inviteRequest.RecieverLogin,
                BandLogin = inviteRequest.BandLogin,
                BandName = inviteRequest.BandName,
                InvitedMusicianLogin = inviteRequest.InvitedMusicianLogin,
                Answer = answer.Answer
            };

            ConfirmBandInvite(confirmation);
        }

        public void NotifyBandInvite(BandInviteNotification notification)
        {
            var @event = new NotificationRecievedEvent()
            {
                NotificationBase = notification
            };
            _eventBus.PublishAsync(@event);
        }

        public void ConfirmBandInvite(BandInviteConfirmationNotification notification)
        {
            var @event = new NotificationRecievedEvent()
            {
                NotificationBase = notification
            };

            _eventBus.PublishAsync(@event);
        }
    }
}
