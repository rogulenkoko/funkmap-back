using System;
using System.IO;
using System.Threading.Tasks;
using Funkmap.Common.Cqrs.Abstract;
using Funkmap.Messenger.Command.Abstract;
using Funkmap.Messenger.Command.Commands;
using Funkmap.Messenger.Contracts.Events;
using Funkmap.Messenger.Contracts.Events.Dialogs;
using Funkmap.Messenger.Entities;
using Funkmap.Messenger.Entities.Mappers;

namespace Funkmap.Messenger.Command.CommandHandlers
{
    internal class CreateDialogCommandHandler : ICommandHandler<CreateDialogCommand>
    {
        private readonly IMessengerCommandRepository _messengerRepository;
        private readonly IEventBus _eventBus;

        public CreateDialogCommandHandler(IMessengerCommandRepository messengerRepository, IEventBus eventBus)
        {
            _messengerRepository = messengerRepository;
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
                    var existingDialog = await _messengerRepository.GetDialogByParticipants(command.Participants);
                    if (existingDialog != null)
                    {
                        await _eventBus.PublishAsync(new DialogCreatedEvent() { Dialog = existingDialog.ToDialog(), Sender = command.CreatorLogin });
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
                await _eventBus.PublishAsync(new DialogCreatedEvent() { Dialog = dialog.ToDialog(), Sender = dialog.CreatorLogin });
            }
            catch (InvalidDataException e)
            {
                var error = $"{nameof(CreateDialogCommand)} validation failed.";
                await _eventBus.PublishAsync(new DialogCreationFailedEvent() { Error = error });
                await _eventBus.PublishAsync(new MessengerCommandFailedEvent()
                {
                    Error = error,
                    ExceptionMessage = e.Message,
                    Sender = command.CreatorLogin
                });
            }
            catch (Exception e)
            {
                var error = "Dialog creation failed.";
                await _eventBus.PublishAsync(new DialogCreationFailedEvent() { Error = e.Message });
                await _eventBus.PublishAsync(new MessengerCommandFailedEvent()
                {
                    Error = error,
                    ExceptionMessage = e.Message,
                    Sender = command.CreatorLogin
                });
            }
        }
    }
}
