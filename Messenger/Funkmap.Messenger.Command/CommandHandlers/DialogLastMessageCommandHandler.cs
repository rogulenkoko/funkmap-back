using System;
using System.IO;
using System.Threading.Tasks;
using Funkmap.Common.Cqrs.Abstract;
using Funkmap.Messenger.Command.Abstract;
using Funkmap.Messenger.Command.Commands;
using Funkmap.Messenger.Contracts.Events;
using Funkmap.Messenger.Contracts.Events.Dialogs;
using Funkmap.Messenger.Entities.Mappers;

namespace Funkmap.Messenger.Command.CommandHandlers
{
    internal class DialogLastMessageCommandHandler : ICommandHandler<UpdateDialogLastMessageCommand>
    {
        private readonly IMessengerCommandRepository _messengerRepository;
        private readonly IEventBus _eventBus;

        public DialogLastMessageCommandHandler(IMessengerCommandRepository messengerRepository,
                                               IEventBus eventBus)
        {
            _messengerRepository = messengerRepository;
            _eventBus = eventBus;
        }

        public async Task Execute(UpdateDialogLastMessageCommand command)
        {
            try
            {
                if (String.IsNullOrEmpty(command.DialogId))
                {
                    throw new InvalidDataException("Command validation failed.");
                }

                var dialog = await _messengerRepository.UpdateLastMessageAsync(command.DialogId, command.Message.ToEntity());

                if (dialog == null)
                {
                    throw new InvalidDataException("Dialog doesn't exist.");
                }

                await _eventBus.PublishAsync(new DialogUpdatedEvent(dialog.ToDialog()));
            }
            catch (InvalidDataException ex)
            {
                var error = $"{nameof(UpdateDialogLastMessageCommand)} validation failed.";
                await _eventBus.PublishAsync(new MessengerCommandFailedEvent() { Error = error, ExceptionMessage = ex.Message});
            }
            catch (Exception ex)
            {
                var error = "Dialog last messages update failed.";
                await _eventBus.PublishAsync(new MessengerCommandFailedEvent() { Error = error, ExceptionMessage = ex.Message});
            }

        }
    }
}
