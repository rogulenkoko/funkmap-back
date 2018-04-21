using System.Threading.Tasks;
using Funkmap.Common.Cqrs.Abstract;
using Funkmap.Messenger.Command.Commands;
using Funkmap.Messenger.Events.Messages;

namespace Funkmap.Messenger.Command.EventHandlers
{
    public class DialogLastMessageEventHandler : IEventHandler<MessageSavedCompleteEvent>
    {
        private readonly IEventBus _eventBus;
        private readonly ICommandBus _commandBus;

        public DialogLastMessageEventHandler(IEventBus eventBus, ICommandBus commandBus)
        {
            _eventBus = eventBus;
            _commandBus = commandBus;
        }

        public void InitHandlers()
        {
            _eventBus.Subscribe<MessageSavedCompleteEvent>(Handle);
        }

        public async Task Handle(MessageSavedCompleteEvent @event)
        {
            var command = new UpdateDialogLastMessageCommand(@event.Message.DialogId.ToString(), @event.Message.DateTimeUtc)
            {
                Message = @event.Message
            };
            await _commandBus.ExecuteAsync(command);
        }


    }
}
