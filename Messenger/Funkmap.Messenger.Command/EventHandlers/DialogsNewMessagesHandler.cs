using System.Threading.Tasks;
using Funkmap.Common.Cqrs.Abstract;
using Funkmap.Messenger.Events.Messages;

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
