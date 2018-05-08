using System;
using System.IO;
using System.Threading.Tasks;
using Funkmap.Common.Cqrs;
using Funkmap.Common.Cqrs.Abstract;
using Funkmap.Common.Logger;
using Funkmap.Messenger.Command.Abstract;
using Funkmap.Messenger.Command.Commands;
using Funkmap.Messenger.Contracts.Events;
using Funkmap.Messenger.Contracts.Events.Dialogs;

namespace Funkmap.Messenger.Command.CommandHandlers
{
    internal class LeaveDialogCommandHandler : ICommandHandler<LeaveDialogCommand>
    {

        private readonly IMessengerCommandRepository _messengerRepository;
        private readonly IEventBus _eventBus;

        public LeaveDialogCommandHandler(IMessengerCommandRepository messengerRepository, IEventBus eventBus)
        {
            _messengerRepository = messengerRepository;
            _eventBus = eventBus;
        }

        public async Task Execute(LeaveDialogCommand command)
        {
            try
            {
                if (String.IsNullOrEmpty(command.DialogId))
                {
                    throw new InvalidDataException("Invalid dialog id.");
                }

                var dialog = await _messengerRepository.GetDialogAsync(command.DialogId);

                if (dialog == null)
                {
                    throw new InvalidDataException("Dialog doesn't exist.");
                }

                if (dialog.CreatorLogin != command.UserLogin && command.UserLogin != command.LeavedUserLogin)
                {
                    throw new InvalidDataException($"{command.UserLogin} can't remove {command.LeavedUserLogin} from dialog {command.DialogId}.");
                }

                if (!dialog.Participants.Contains(command.LeavedUserLogin))
                {
                    throw new InvalidDataException($"{command.LeavedUserLogin} is not a member of dialog {command.DialogId}.");
                }

                dialog.Participants.Remove(command.LeavedUserLogin);
                await _messengerRepository.UpdateDialogAsync(dialog);

                await _eventBus.PublishAsync(new UserLeavedDialogEvent(dialog.Id.ToString(), command.LeavedUserLogin,
                    command.UserLogin, dialog.CreatorLogin));
            }
            catch (InvalidDataException ex)
            {
                var error = $"{nameof(LeaveDialogCommand)} validation failed.";
                await _eventBus.PublishAsync(new MessengerCommandFailedEvent()
                {
                    Error = error,
                    ExceptionMessage = ex.Message,
                    Sender = command.UserLogin
                });
            }
            catch (Exception e)
            {
                var error = "Leaving dialog failed.";
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

