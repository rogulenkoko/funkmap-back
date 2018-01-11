using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Common.Cqrs.Abstract;
using Funkmap.Common.Tools;
using Funkmap.Messenger.Command.Commands;
using Funkmap.Messenger.Events.Dialogs;

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

        public void Handle(UserInvitedToDialogEvent @event)
        {
            string addedParticipantsString = @event.InvitedParticipants.Count == 1
                ? @event.InvitedParticipants.First()
                : String.Join(", ", @event.InvitedParticipants);

            var command = new SaveMessageCommand()
            {
                DialogId = @event.Dialog.Id.ToString(),
                Sender = FunkmapConstants.FunkmalAdminUser,
                Text = $"{@event.UserLogin} пригласил {addedParticipantsString}"//todo подумать о локализации
            };

            _commandBus.Execute(command);
        }

        
    }
}
