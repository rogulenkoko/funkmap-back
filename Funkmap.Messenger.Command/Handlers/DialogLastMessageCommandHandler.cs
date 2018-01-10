using System;
using System.Threading.Tasks;
using Funkmap.Common.Cqrs.Abstract;
using Funkmap.Common.Logger;
using Funkmap.Messenger.Command.Commands;
using Funkmap.Messenger.Command.Repositories;
using Funkmap.Messenger.Events.Dialogs;

namespace Funkmap.Messenger.Command.Handlers
{
    internal class DialogLastMessageCommandHandler : ICommandHandler<UpdateDialogLastMessageCommand>
    {
        private readonly IMessengerCommandRepository _messengerRepository;
        private readonly IFunkmapLogger<SaveMessageCommandHandler> _logger;
        private readonly IEventBus _eventBus;

        public DialogLastMessageCommandHandler(IMessengerCommandRepository messengerRepository, IFunkmapLogger<SaveMessageCommandHandler> logger, IEventBus eventBus)
        {
            _messengerRepository = messengerRepository;
            _logger = logger;
            _eventBus = eventBus;
        }

        public async Task Execute(UpdateDialogLastMessageCommand command)
        {
            if (String.IsNullOrEmpty(command.DialogId))
            {
                return;
            }

            var dialog = await _messengerRepository.UpdateLastMessageDate(command.DialogId, command.Message.DateTimeUtc);
            dialog.LastMessage = command.Message;
            await _eventBus.PublishAsync(new DialogUpdatedEvent() {Dialog = dialog});
        }
    }
}
