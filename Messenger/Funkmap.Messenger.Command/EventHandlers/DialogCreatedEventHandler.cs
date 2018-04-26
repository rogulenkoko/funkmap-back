using System.Threading.Tasks;
using Funkmap.Common.Cqrs.Abstract;
using Funkmap.Messenger.Command.Commands;
using Funkmap.Messenger.Contracts.Events.Dialogs;
using Funkmap.Messenger.Entities;

namespace Funkmap.Messenger.Command.EventHandlers
{
    public class DialogCreatedEventHandler : IEventHandler<DialogCreatedEvent>
    {
        private readonly IEventBus _eventBus;
        private readonly ICommandBus _commandBus;

        public DialogCreatedEventHandler(IEventBus eventBus, ICommandBus commandBus)
        {
            _eventBus = eventBus;
            _commandBus = commandBus;
        }

        public void InitHandlers()
        {
            _eventBus.Subscribe<DialogCreatedEvent>(Handle);
        }

        public async Task Handle(DialogCreatedEvent @event)
        {
            if (@event.Dialog.Participants.Count > 2)
            {
                var saveMessageCommand = new SaveMessageCommand()
                {
                    DialogId = @event.Dialog.Id.ToString(),
                    Sender = @event.Sender,
                    MessageType = MessageType.System,
                    //todo подумать о локализации
                    Text = $"{@event.Dialog.CreatorLogin} создал беседу {@event.Dialog.Name} из {@event.Dialog.Participants.Count} участников"
                };
                await _commandBus.ExecuteAsync(saveMessageCommand);
            }
        }
        
    }
}
