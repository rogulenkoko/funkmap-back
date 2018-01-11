using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Funkmap.Common.Cqrs.Abstract;
using Funkmap.Common.Logger;
using Funkmap.Messenger.Command.Commands;
using Funkmap.Messenger.Command.Repositories;
using Funkmap.Messenger.Events.Dialogs;

namespace Funkmap.Messenger.Command.CommandHandlers
{
    internal class InviteParticipantsCommandHandler : ICommandHandler<InviteParticipantsCommand>
    {
        private readonly IMessengerCommandRepository _messengerRepository;
        private readonly IFunkmapLogger<InviteParticipantsCommandHandler> _logger;
        private readonly IEventBus _eventBus;

        public InviteParticipantsCommandHandler(IMessengerCommandRepository messengerRepository, IFunkmapLogger<InviteParticipantsCommandHandler> logger, IEventBus eventBus)
        {
            _messengerRepository = messengerRepository;
            _logger = logger;
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

                var newParticipants = command.InvitedUsers.Except(dialog.Participants).ToList();

                if (!newParticipants.Any())
                {
                    throw new InvalidDataException("There are not invited users");
                }

                dialog.Participants.AddRange(newParticipants);

                await _messengerRepository.UpdateDialogAsync(dialog);

                await _eventBus.PublishAsync(new UserInvitedToDialogEvent(dialog, command.UserLogin, newParticipants));


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
