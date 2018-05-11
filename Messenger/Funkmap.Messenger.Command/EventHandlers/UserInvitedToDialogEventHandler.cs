using System;
using System.Linq;
using System.Threading.Tasks;
using Funkmap.Cqrs.Abstract;
using Funkmap.Messenger.Command.Commands;
using Funkmap.Messenger.Contracts.Events.Dialogs;
using Funkmap.Messenger.Entities;

namespace Funkmap.Messenger.Command.EventHandlers
{
    public class UserInvitedToDialogEventHandler : IEventHandler<UserInvitedToDialogEvent>
    {
        private readonly IEventBus _eventBus;
        private readonly ICommandBus _commandBus;

        public UserInvitedToDialogEventHandler(IEventBus eventBus, ICommandBus commandBus)
        {
            _eventBus = eventBus;
            _commandBus = commandBus;
        }

        public void InitHandlers()
        {
            _eventBus.Subscribe<UserInvitedToDialogEvent>(Handle);
        }

        public async Task Handle(UserInvitedToDialogEvent @event)
        {
            string addedParticipantsString = @event.InvitedParticipants.Count == 1
                ? @event.InvitedParticipants.First()
                : String.Join(", ", @event.InvitedParticipants);

            var command = new SaveMessageCommand
            {
                DialogId = @event.Dialog.Id.ToString(),
                Sender = @event.UserLogin,
                MessageType = MessageType.System,
                Text = $"{@event.UserLogin} пригласил {addedParticipantsString}"//todo подумать о локализации
            };

            await _commandBus.ExecuteAsync(command);
        }

        
    }
}
