using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Funkmap.Common.Cqrs.Abstract;
using Funkmap.Messenger.Events.Dialogs;
using Funkmap.Messenger.Events.Messages;
using Funkmap.Messenger.Hubs;
using Funkmap.Messenger.Hubs.Abstract;
using Funkmap.Messenger.Mappers;
using Funkmap.Messenger.Models;
using Funkmap.Messenger.Query.Queries;
using Funkmap.Messenger.Query.Responses;
using Funkmap.Messenger.Services.Abstract;
using Microsoft.AspNet.SignalR;

namespace Funkmap.Messenger.Handlers
{
    public class SignalrEventHandler : IEventHandler<DialogUpdatedEvent>, 
                                       IEventHandler<MessageSavedCompleteEvent>, 
                                       IEventHandler<MessagesReadEvent>,
                                       IEventHandler<DialogCreatedEvent>,
                                       IEventHandler<ContentUploadedEvent>
    {
        private readonly IEventBus _eventBus;
        private readonly IMessengerConnectionService _connectionService;
        private readonly IQueryContext _queryContext;

        public SignalrEventHandler(IEventBus eventBus, IMessengerConnectionService connectionService, IQueryContext queryContext)
        {
            _eventBus = eventBus;
            _connectionService = connectionService;
            _queryContext = queryContext;
        }

        

        public void InitHandlers()
        {
            _eventBus.Subscribe<DialogUpdatedEvent>(Handle);
            _eventBus.Subscribe<MessageSavedCompleteEvent>(Handle);
            _eventBus.Subscribe<MessagesReadEvent>(Handle);
            _eventBus.Subscribe<DialogCreatedEvent>(Handle);
            _eventBus.Subscribe<ContentUploadedEvent>(Handle);
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

        public void Handle(DialogCreatedEvent @event)
        {
            var login = @event.Sender;

            var clientIds = _connectionService.GetConnectionIdsByLogin(login).ToList();

            var query = new UserDialogQuery(@event.Dialog.Id.ToString(), login);
            var response = _queryContext.Execute<UserDialogQuery, UserDialogResponse>(query).GetAwaiter().GetResult();

            if (!response.Success) return;

            GlobalHost.ConnectionManager.GetHubContext<MessengerHub, IMessengerHub>()
                .Clients.Clients(clientIds)
                .OnDialogCreated(response.Dialog.ToModel(login));
        }

        public void Handle(MessagesReadEvent @event)
        {
            var connectionIds = _connectionService.GetConnectionIdsByLogins(@event.DialogMembers).ToList();

            var readModel = new DialogReadModel() {DialogId = @event.DialogId, UserWhoRead = @event.UserLogin};

            GlobalHost.ConnectionManager.GetHubContext<MessengerHub, IMessengerHub>().Clients.Clients(connectionIds).OnDialogRead(readModel);
        }

        public void Handle(DialogUpdatedEvent @event)
        {
            var clientIds = _connectionService.GetConnectionIdsByLogins(@event.Dialog.Participants).ToList();
            

            Parallel.ForEach(clientIds, clientId =>
            {
                var login = _connectionService.GetLoginByConnectionId(clientId);

                var query = new UserDialogQuery(@event.Dialog.Id.ToString(), login);
                var response = _queryContext.Execute<UserDialogQuery, UserDialogResponse>(query).GetAwaiter().GetResult();

                if(!response.Success) return;

                GlobalHost.ConnectionManager.GetHubContext<MessengerHub, IMessengerHub>()
                    .Clients.Client(clientId)
                    .OnDialogUpdated(response.Dialog.ToModel(login));
            });
        }

        public void Handle(ContentUploadedEvent @event)
        {
            var connectionIds = _connectionService.GetConnectionIdsByLogin(@event.Sender);

            GlobalHost.ConnectionManager.GetHubContext<MessengerHub, IMessengerHub>()
                .Clients.Clients(connectionIds.ToList())
                .OnContentLoaded(new ContentItemModel() {Name = @event.Name, DataUrl = @event.DataUrl, ContentType = @event.ContentType});

        }
    }
}

