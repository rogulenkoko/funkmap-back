using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Funkmap.Common.Cqrs.Abstract;
using Funkmap.Messenger.Command.Abstract;
using Funkmap.Messenger.Command.Commands;
using Funkmap.Messenger.Contracts.Events;
using Funkmap.Messenger.Contracts.Events.Dialogs;
using Funkmap.Messenger.Entities.Mappers;

namespace Funkmap.Messenger.Command.CommandHandlers
{
    internal class InviteParticipantsCommandHandler : ICommandHandler<InviteParticipantsCommand>
    {
        private readonly IMessengerCommandRepository _messengerRepository;
        private readonly IEventBus _eventBus;

        public InviteParticipantsCommandHandler(IMessengerCommandRepository messengerRepository, IEventBus eventBus)
        {
            _messengerRepository = messengerRepository;
            _eventBus = eventBus;
        }

        public async Task Execute(InviteParticipantsCommand command)
        {
            try
            {
                if (String.IsNullOrEmpty(command.DialogId))
                {
                    throw new InvalidDataException("Invalid dialog id");
                }

                if (command.InvitedUsers == null || command.InvitedUsers.Count == 0)
                {
                    throw new InvalidDataException("There are not invited users");
                }

                if (String.IsNullOrEmpty(command.UserLogin))
                {
                    throw new InvalidDataException("Invalid user login");
                }

                var dialog = await _messengerRepository.GetDialogAsync(command.DialogId);

                if (dialog == null)
                {
                    throw new InvalidDataException("Dialog is not exist");
                }

                var newParticipants = command.InvitedUsers.Except(dialog.Participants).ToList();

                if (!newParticipants.Any())
                {
                    throw new InvalidDataException("There are not invited users");
                }

                dialog.Participants.AddRange(newParticipants);

                await _messengerRepository.UpdateDialogAsync(dialog);

                await _eventBus.PublishAsync(new UserInvitedToDialogEvent(dialog.ToDialog(), command.UserLogin, newParticipants));


            }
            catch (InvalidDataException ex)
            {
                var error = $"{nameof(InviteParticipantsCommand)} validation failed.";
                await _eventBus.PublishAsync(new MessengerCommandFailedEvent()
                {
                    Error = error,
                    ExceptionMessage = ex.Message,
                    Sender = command.UserLogin
                });
            }
            catch (Exception e)
            {
                var error = "Invitation failed.";
                await _eventBus.PublishAsync(new MessengerCommandFailedEvent()
                {
                    Error = error,
                    ExceptionMessage = e.Message,
                    Sender = command.UserLogin
                });
            }
        }
    }
}
