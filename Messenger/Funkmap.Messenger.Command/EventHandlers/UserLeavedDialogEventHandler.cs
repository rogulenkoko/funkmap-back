using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Common.Cqrs.Abstract;
using Funkmap.Common.Tools;
using Funkmap.Messenger.Command.Commands;
using Funkmap.Messenger.Entities;
using Funkmap.Messenger.Events.Dialogs;

namespace Funkmap.Messenger.Command.EventHandlers
{
    public class UserLeavedDialogEventHandler : IEventHandler<UserLeavedDialogEvent>
    {
        private readonly IEventBus _eventBus;
        private readonly ICommandBus _commandBus;

        public UserLeavedDialogEventHandler(IEventBus eventBus, ICommandBus commandBus)
        {
            _eventBus = eventBus;
            _commandBus = commandBus;
        }

        public void InitHandlers()
        {
            _eventBus.Subscribe<UserLeavedDialogEvent>(Handle);
        }

        public async Task Handle(UserLeavedDialogEvent @event)
        {
            SaveMessageCommand command;

            if (@event.DialogCreatorLogin == @event.UserLogin && @event.UserLogin != @event.LeavedUserLogin)
            {
                command = new SaveMessageCommand()
                {
                    DialogId = @event.DialogId,
                    Sender = @event.UserLogin,
                    MessageType = MessageType.System,
                    Text = $"{ @event.UserLogin} исключил {@event.LeavedUserLogin} из беседы"
                };
            }
            else
            {
                command = new SaveMessageCommand()
                {
                    DialogId = @event.DialogId,
                    Sender = @event.UserLogin,
                    MessageType = MessageType.System,
                    Text = $"{@event.UserLogin} покинул беседу"
                };
            }

            await _commandBus.ExecuteAsync(command);
        }
    }
}
