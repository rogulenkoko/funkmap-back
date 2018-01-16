using System;
using System.IO;
using System.Threading.Tasks;
using Funkmap.Common.Cqrs.Abstract;
using Funkmap.Common.Logger;
using Funkmap.Messenger.Command.Abstract;
using Funkmap.Messenger.Command.Commands;
using Funkmap.Messenger.Events.Dialogs;

namespace Funkmap.Messenger.Command.CommandHandlers
{
    internal class LeaveDialogCommandHandler : ICommandHandler<LeaveDialogCommand>
    {

        private readonly IMessengerCommandRepository _messengerRepository;
        private readonly IFunkmapLogger<LeaveDialogCommandHandler> _logger;
        private readonly IEventBus _eventBus;

        public LeaveDialogCommandHandler(IMessengerCommandRepository messengerRepository, IFunkmapLogger<LeaveDialogCommandHandler> logger, IEventBus eventBus)
        {
            _messengerRepository = messengerRepository;
            _logger = logger;
            _eventBus = eventBus;
        }

        public async Task Execute(LeaveDialogCommand command)
        {
            try
            {
                if (String.IsNullOrEmpty(command.DialogId))
                {
                    throw new InvalidDataException("Invalid dialog id");
                }

                var dialog = await _messengerRepository.GetDialogAsync(command.DialogId);

                if (dialog == null)
                {
                    throw new InvalidDataException("Dialog is not exist");
                }

                if (dialog.CreatorLogin != command.UserLogin && command.UserLogin != command.LeavedUserLogin)
                {
                    throw new InvalidDataException($"{command.UserLogin} can't remove {command.LeavedUserLogin} from dialog {command.DialogId}");
                }

                if (!dialog.Participants.Contains(command.LeavedUserLogin))
                {
                    throw new InvalidDataException($"{command.LeavedUserLogin} is not a member of dialog {command.DialogId}");
                }

                dialog.Participants.Remove(command.LeavedUserLogin);
                await _messengerRepository.UpdateDialogAsync(dialog);

                await _eventBus.PublishAsync(new UserLeavedDialogEvent(dialog.Id.ToString(), command.LeavedUserLogin,
                    command.UserLogin, dialog.CreatorLogin));
            }
            catch (InvalidDataException ex)
            {
                _logger.Error(ex, "Validation failed");
            }
            catch (Exception e)
            {
                _logger.Error(e);
            }
            

        }
    }
}

