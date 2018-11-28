using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Funkmap.Cqrs;
using Funkmap.Cqrs.Abstract;
using Funkmap.Domain.Abstract.Repositories;
using Funkmap.Domain.Models;
using Funkmap.Domain.Notifications.BandInvite;
using Funkmap.Domain.Parameters;
using Funkmap.Logger;
using Funkmap.Notifications.Contracts;
using Funkmap.Notifications.Contracts.Abstract;
using Funkmap.Notifications.Contracts.Models;
using Newtonsoft.Json;

namespace Funkmap.Domain.Services
{
    public class FunkmapNotificationHandler : IEventHandler<NotificationAnswer>
    {
        private readonly IEventBus _eventBus;
        private readonly IFunkmapLogger<FunkmapNotificationHandler> _logger;
        private readonly IBaseCommandRepository _commandRepository;
        private readonly IBaseQueryRepository _queryRepository;
        private readonly IFunkmapNotificationService _notificationService;

        public FunkmapNotificationHandler(IBaseCommandRepository commandRepository,
                                          IBaseQueryRepository queryRepository,
                                          IEventBus eventBus,
                                          IFunkmapNotificationService notificationService,
                                          IFunkmapLogger<FunkmapNotificationHandler> logger)
        {
            _eventBus = eventBus;
            _notificationService = notificationService;
            _commandRepository = commandRepository;
            _queryRepository = queryRepository;
            _logger = logger;
        }

        public void InitHandlers()
        {
            var options = new MessageQueueOptions
            {
                SpecificKey = $"{typeof(BandInviteNotification).GetCustomAttribute<FunkmapNotificationAttribute>().Name}_answer"
            };

            _eventBus.Subscribe<NotificationAnswer>(Handle, options);
        }

        public async Task Handle(NotificationAnswer answer)
        {
            var inviteRequest = JsonConvert.DeserializeObject<BandInviteNotification>(answer.NotificationJson);
            
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
                UserLogin = answer.Sender,
                Parameter = bandUpdate
            };

            var updateResult = await _commandRepository.UpdateAsync(updateParameter);

            if (!updateResult.Success)
            {
                return;
            }

            var confirmation = new BandInviteConfirmationNotification
            {
                BandLogin = inviteRequest.BandLogin,
                BandName = inviteRequest.BandName,
                InvitedMusicianLogin = inviteRequest.InvitedMusicianLogin,
                Answer = answer.Answer
            };

            await _notificationService.NotifyAsync(confirmation, answer.Receiver, answer.Sender);
        }
    }
}
