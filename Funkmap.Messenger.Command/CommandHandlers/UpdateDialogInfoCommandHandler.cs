using System;
using System.IO;
using System.Threading.Tasks;
using Funkmap.Common.Cqrs.Abstract;
using Funkmap.Common.Data.Mongo.Entities;
using Funkmap.Common.Logger;
using Funkmap.Common.Tools;
using Funkmap.Messenger.Command.Commands;
using Funkmap.Messenger.Command.Repositories;
using Funkmap.Messenger.Events.Dialogs;

namespace Funkmap.Messenger.Command.CommandHandlers
{
    internal class UpdateDialogInfoCommandHandler : ICommandHandler<UpdateDialogInfoCommand>
    {
        private readonly IMessengerCommandRepository _messengerRepository;
        private readonly IFunkmapLogger<UpdateDialogInfoCommandHandler> _logger;
        private readonly IEventBus _eventBus;

        public UpdateDialogInfoCommandHandler(IMessengerCommandRepository messengerRepository, IFunkmapLogger<UpdateDialogInfoCommandHandler> logger, IEventBus eventBus)
        {
            _messengerRepository = messengerRepository;
            _logger = logger;
            _eventBus = eventBus;
        }

        public async Task Execute(UpdateDialogInfoCommand command)
        {
            try
            {
                if ((command.Avatar == null || command.Avatar.Length == 0) && String.IsNullOrEmpty(command.Name))
                {
                    throw new InvalidDataException("Nothing to update");
                }

                if (String.IsNullOrEmpty(command.DialogId))
                {
                    throw new InvalidDataException("Invalid dialog id");
                }

                var dialog = await _messengerRepository.GetDialogAsync(command.DialogId);

                if (dialog == null)
                {
                    throw new InvalidDataException("Dialog is not exist");
                }

                if (!dialog.Participants.Contains(command.UserLogin))
                {
                    throw new InvalidDataException($"{command.UserLogin} can not modify dialog {command.DialogId}");
                }

                if (command.Avatar != null && command.Avatar.Length != 0)
                {
                    var cutted = FunkmapImageProcessor.MinifyImage(command.Avatar);
                    dialog.Avatar = new ImageInfo() {Image = cutted };
                }

                if (!String.IsNullOrEmpty(command.Name))
                {
                    dialog.Name = command.Name;
                }

                await _messengerRepository.UpdateDialogAsync(dialog);
                await _eventBus.PublishAsync(new DialogUpdatedEvent(dialog));

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
