using System;
using System.Threading.Tasks;
using Funkmap.Common.Cqrs.Abstract;
using Funkmap.Common.Logger;
using Funkmap.Messenger.Command.Commands;
using Funkmap.Messenger.Command.Repositories;
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
            if (String.IsNullOrEmpty(command.DialogId))
            {
                return;
            }

            var dialog = await _messengerRepository.GetDialogAsync(command.DialogId);

            if (dialog.CreatorLogin != command.UserLogin && command.UserLogin != command.LeavedUserLogin)
            {
                _logger.Info($"{command.UserLogin} can't remove {command.LeavedUserLogin} from dialog {command.DialogId}");
                return;
            }

            if (!dialog.Participants.Contains(command.LeavedUserLogin))
            {
                _logger.Info($"{command.LeavedUserLogin} is not a member of dialog {command.DialogId}");
                return;
            }

            dialog.Participants.Remove(command.LeavedUserLogin);
            await _messengerRepository.UpdateDialogAsync(dialog);

            await _eventBus.PublishAsync(new UserLeavedDialogEvent(dialog.Id.ToString(), command.LeavedUserLogin,
                command.UserLogin, dialog.CreatorLogin));

        }
    }
}

