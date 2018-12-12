using System;
using System.Threading.Tasks;
using Funkmap.Cqrs.Abstract;
using Funkmap.Messenger.Contracts.Events;
using Funkmap.Messenger.Contracts.Events.Dialogs;
using Funkmap.Messenger.Contracts.Events.Messages;
using Funkmap.Messenger.Hubs;
using Funkmap.Messenger.Hubs.Abstract;
using Funkmap.Messenger.Mappers;
using Funkmap.Messenger.Models;
using Funkmap.Messenger.Query.Queries;
using Funkmap.Messenger.Query.Responses;
using Funkmap.Messenger.Services.Abstract;
using Microsoft.AspNetCore.SignalR;

namespace Funkmap.Messenger.Handlers
{
    public class SignalrEventHandler : IEventHandler<DialogUpdatedEvent>, 
                                       IEventHandler<MessageSavedCompleteEvent>, 
                                       IEventHandler<MessagesReadEvent>,
                                       IEventHandler<DialogCreatedEvent>,
                                       IEventHandler<ContentUploadedEvent>,
                                       IEventHandler<MessengerCommandFailedEvent>
    {
        private readonly IEventBus _eventBus;
        private readonly IMessengerConnectionService _connectionService;
        private readonly IQueryContext _queryContext;
        private readonly IHubContext<MessengerHub, IMessengerHub> _hubContext;

        public SignalrEventHandler(IEventBus eventBus, 
                                   IMessengerConnectionService connectionService, 
                                   IQueryContext queryContext,
                                   IHubContext<MessengerHub, IMessengerHub> hubContext)
        {
            _eventBus = eventBus;
            _connectionService = connectionService;
            _queryContext = queryContext;
            _hubContext = hubContext;
        }
        

        public void InitHandlers()
        {
            _eventBus.Subscribe<DialogUpdatedEvent>(Handle);
            _eventBus.Subscribe<MessageSavedCompleteEvent>(Handle);
            _eventBus.Subscribe<MessagesReadEvent>(Handle);
            _eventBus.Subscribe<DialogCreatedEvent>(Handle);
            _eventBus.Subscribe<ContentUploadedEvent>(Handle);
            _eventBus.Subscribe<MessengerCommandFailedEvent>(Handle);
        }

        public async Task Handle(MessageSavedCompleteEvent @event)
        {
            await Task.Yield();

            var message = @event.Message.ToModel();

            @event.DialogParticipants.ForEach(login =>
            {
                var connectionId = _connectionService.GetConnectionIdsByLogin(login);
                _hubContext
                    .Clients.Clients(connectionId)
                    .OnMessageSent(message);
            });
        }

        public async Task Handle(DialogCreatedEvent @event)
        {
            var login = @event.Sender;

            var clientIds = _connectionService.GetConnectionIdsByLogin(login);

            var query = new UserDialogQuery(@event.Dialog.Id, login);
            var response = await _queryContext.ExecuteAsync<UserDialogQuery, UserDialogResponse>(query);

            if (!response.Success) return;

            await _hubContext
                .Clients.Clients(clientIds)
                .OnDialogCreated(response.Dialog.ToModel(login));
        }

        public async Task Handle(MessagesReadEvent @event)
        {
            var connectionIds = _connectionService.GetConnectionIdsByLogins(@event.DialogMembers);

            var readModel = new DialogReadModel() {DialogId = @event.DialogId, UserWhoRead = @event.UserLogin};

            await _hubContext.Clients.Clients(connectionIds).OnDialogRead(readModel);
        }

        public async Task Handle(DialogUpdatedEvent @event)
        {

            await Task.Yield();

            var clientIds = _connectionService.GetConnectionIdsByLogins(@event.Dialog.Participants);
            
            Parallel.ForEach(clientIds, async clientId =>
            {
                var login = _connectionService.GetLoginByConnectionId(clientId);

                var query = new UserDialogQuery(@event.Dialog.Id.ToString(), login);
                var response = await _queryContext.ExecuteAsync<UserDialogQuery, UserDialogResponse>(query);

                if(!response.Success) return;

                await _hubContext
                    .Clients.Client(clientId)
                    .OnDialogUpdated(response.Dialog.ToModel(login));
            });
        }

        public async Task Handle(ContentUploadedEvent @event)
        {
            var connectionIds = _connectionService.GetConnectionIdsByLogin(@event.Sender);

            await _hubContext
                .Clients.Clients(connectionIds)
                .OnContentLoaded(new ContentItemModel() {Name = @event.Name, DataUrl = @event.DataUrl, ContentType = @event.ContentType});

        }

        public async Task Handle(MessengerCommandFailedEvent @event)
        {
            if (String.IsNullOrEmpty(@event.Sender)) return;

            var connectionIds = _connectionService.GetConnectionIdsByLogin(@event.Sender);

            await _hubContext
                .Clients.Clients(connectionIds)
                .OnError(new CommandErrorModel() {Error = @event.Error, ExceptionMessage = @event.ExceptionMessage});
        }
    }
}

