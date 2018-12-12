using System.Threading.Tasks;
using Funkmap.Cqrs.Abstract;
using Funkmap.Messenger.Contracts.Events.Messages;

namespace Funkmap.Messenger.Command.EventHandlers
{
    public class DialogsNewMessagesHandler : IEventHandler<MessageSavedCompleteEvent>
    {
        public void InitHandlers()
        {
            
        }

        public async Task Handle(MessageSavedCompleteEvent @event)
        {
            
        }
    }
}
