using System;
using System.Threading.Tasks;
using Funkmap.Common.Cqrs.Abstract;
using Funkmap.Common.Logger;
using Funkmap.Messenger.Command.Commands;
using Funkmap.Messenger.Command.Repositories;
using Funkmap.Messenger.Events.Dialogs;

namespace Funkmap.Messenger.Command.CommandHandlers
{
    internal class DialogLastMessageCommandHandler : ICommandHandler<UpdateDialogLastMessageCommand>
    {
        private readonly IMessengerCommandRepository _messengerRepository;
        private readonly IFunkmapLogger<DialogLastMessageCommandHandler> _logger;
        private readonly IEventBus _eventBus;

        public DialogLastMessageCommandHandler(IMessengerCommandRepository messengerRepository, 
                                               IFunkmapLogger<DialogLastMessageCommandHandler> logger, 
                                               IEventBus eventBus)
        {
            _messengerRepository = messengerRepository;
            _logger = logger;
            _eventBus = eventBus;
        }

        public async Task Execute(UpdateDialogLastMessageCommand command)
        {
            if (String.IsNullOrEmpty(command.DialogId))
            {
                _logger.Info("Command validation failed");
                return;
            }

            try
            {
                var dialog = await _messengerRepository.UpdateLastMessageDateAsync(command.DialogId, command.Message.DateTimeUtc);
                dialog.LastMessage = command.Message;
                await _eventBus.PublishAsync(new DialogUpdatedEvent() { Dialog = dialog });
            }
            catch(Exception ex)
            {
                _logger.Error(ex, "Dialog update failed");
            }
            
        }
    }
}
