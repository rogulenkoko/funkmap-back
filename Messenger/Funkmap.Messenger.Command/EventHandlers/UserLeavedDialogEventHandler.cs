using System.Threading.Tasks;
using Funkmap.Cqrs.Abstract;
using Funkmap.Messenger.Command.Commands;
using Funkmap.Messenger.Contracts.Events.Dialogs;
using Funkmap.Messenger.Entities;

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
