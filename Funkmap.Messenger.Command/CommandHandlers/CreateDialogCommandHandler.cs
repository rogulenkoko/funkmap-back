using System;
using System.IO;
using System.Threading.Tasks;
using Funkmap.Common.Cqrs.Abstract;
using Funkmap.Common.Logger;
using Funkmap.Messenger.Command.Abstract;
using Funkmap.Messenger.Command.Commands;
using Funkmap.Messenger.Entities;
using Funkmap.Messenger.Events.Dialogs;

namespace Funkmap.Messenger.Command.CommandHandlers
{
    internal class CreateDialogCommandHandler : ICommandHandler<CreateDialogCommand>
    {
        private readonly IMessengerCommandRepository _messengerRepository;
        private readonly IFunkmapLogger<CreateDialogCommandHandler> _logger;
        private readonly IEventBus _eventBus;

        public CreateDialogCommandHandler(IMessengerCommandRepository messengerRepository, IFunkmapLogger<CreateDialogCommandHandler> logger, IEventBus eventBus)
        {
            _messengerRepository = messengerRepository;
            _logger = logger;
            _eventBus = eventBus;
        }

        public async Task Execute(CreateDialogCommand command)
        {
            try
            {
                if (command.Participants == null || command.Participants.Count == 0)
                {
                    throw new InvalidDataException("Invalid participants");
                }

                if (command.Participants.Count > 2 && String.IsNullOrEmpty(command.CreatorLogin))
                {
                    throw new InvalidDataException("Invalid creator login. Creator login is necessary when participants count is greater than 2");
                }

                if (command.Participants.Count > 2 && String.IsNullOrEmpty(command.DialogName))
                {
                    throw new InvalidDataException("Invalid dialogName. Dialog name is necessary when participants count is greater than 2");
                }

                if (!command.Participants.Contains(command.CreatorLogin))
                {
                    command.Participants.Add(command.CreatorLogin);
                }

                if (command.Participants.Count == 2)
                {
                    var existingDialog = await _messengerRepository.GetDialogByParticipants(command.Participants.ToArray());
                    if (existingDialog != null)
                    {
                        await _eventBus.PublishAsync(new DialogCreatedEvent() { Dialog = existingDialog, Sender = command.CreatorLogin});
                        return;
                    }
                }
                

                var dialog = new DialogEntity
                {
                    Name = command.DialogName,
                    CreatorLogin = command.CreatorLogin,
                    Participants = command.Participants,
                    DialogType = command.Participants.Count == 2 ? DialogType.Base : DialogType.Conversation
                };

                await _messengerRepository.AddDialogAsync(dialog);
                await _eventBus.PublishAsync(new DialogCreatedEvent() {Dialog = dialog, Sender = dialog.CreatorLogin});
            }
            catch (InvalidDataException e)
            {
                var error = "Command validation failed";
                _logger.Error(e, error);
                await _eventBus.PublishAsync(new DialogCreationFailedEvent() {Error = error});
            }
            catch (Exception e)
            {
                _logger.Error(e);
                await _eventBus.PublishAsync(new DialogCreationFailedEvent() { Error = e.Message });
            }
        }
    }
}
