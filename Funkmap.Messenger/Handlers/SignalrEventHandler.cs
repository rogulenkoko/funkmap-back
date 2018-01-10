using System.Linq;
using Funkmap.Common.Cqrs.Abstract;
using Funkmap.Messenger.Events.Dialogs;
using Funkmap.Messenger.Events.Messages;
using Funkmap.Messenger.Hubs;
using Funkmap.Messenger.Hubs.Abstract;
using Funkmap.Messenger.Mappers;
using Funkmap.Messenger.Services.Abstract;
using Microsoft.AspNet.SignalR;

namespace Funkmap.Messenger.Handlers
{
    public class SignalrEventHandler : IEventHandler<DialogUpdatedEvent>, IEventHandler<MessageSavedCompleteEvent>
    {
        private readonly IEventBus _eventBus;
        private readonly IMessengerConnectionService _connectionService;

        public SignalrEventHandler(IEventBus eventBus, IMessengerConnectionService connectionService)
        {
            _eventBus = eventBus;
            _connectionService = connectionService;
        }
        
        public void InitHandlers()
        {
            _eventBus.Subscribe<DialogUpdatedEvent>(Handle);
            _eventBus.Subscribe<MessageSavedCompleteEvent>(Handle);
        }

        public void Handle(MessageSavedCompleteEvent @event)
        {
            var message = @event.Message.ToModel();

            @event.DialogParticipants.ForEach(login =>
            {
                var connectionId = _connectionService.GetConnectionIdsByLogin(login);
                GlobalHost.ConnectionManager.GetHubContext<MessengerHub, IMessengerHub>()
                    .Clients.Clients(connectionId.ToList())
                    .OnMessageSent(message);
            });
        }

        public void Handle(DialogUpdatedEvent @event)
        {
            var clientIds = _connectionService.GetConnectionIdsByLogins(@event.Dialog.Participants).ToList();

            clientIds.ForEach(clientId =>
            {
                var login = _connectionService.GetLoginByConnectionId(clientId);
                var dialog = @event.Dialog.ToModel(login);
                GlobalHost.ConnectionManager.GetHubContext<MessengerHub, IMessengerHub>()
                    .Clients.Client(clientId)
                    .OnDialogUpdated(dialog);
            });
        }
    }
}
