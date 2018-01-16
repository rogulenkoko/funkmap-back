using System;
using System.IO;
using System.Threading.Tasks;
using Autofac.Features.AttributeFilters;
using Funkmap.Common.Abstract;
using Funkmap.Common.Cqrs.Abstract;
using Funkmap.Common.Logger;
using Funkmap.Common.Tools;
using Funkmap.Messenger.Command.Abstract;
using Funkmap.Messenger.Command.Commands;
using Funkmap.Messenger.Events.Dialogs;

namespace Funkmap.Messenger.Command.CommandHandlers
{
    internal class UpdateDialogInfoCommandHandler : ICommandHandler<UpdateDialogInfoCommand>
    {
        private readonly IMessengerCommandRepository _messengerRepository;
        private readonly IFileStorage _fileStorage;

        private readonly IFunkmapLogger<UpdateDialogInfoCommandHandler> _logger;
        private readonly IEventBus _eventBus;

        public UpdateDialogInfoCommandHandler(IMessengerCommandRepository messengerRepository,
                                              [KeyFilter(MessengerCollectionNameProvider.MessengerStorage)]
                                              IFileStorage fileStorage,
                                              IFunkmapLogger<UpdateDialogInfoCommandHandler> logger, 
                                              IEventBus eventBus)
        {
            _messengerRepository = messengerRepository;
            _fileStorage = fileStorage;
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

                if (command.Avatar != null)
                {
                    string fullPath;

                    if (command.Avatar.Length != 0)
                    {
                        var cutted = FunkmapImageProcessor.MinifyImage(command.Avatar);
                        var date = DateTimeOffset.Now.ToString("yyyyMMddhhmmss");
                        fullPath = await _fileStorage.UploadFromBytesAsync($"{dialog.Id}{date}", cutted);
                    }
                    else
                    {
                        await _fileStorage.DeleteAsync(dialog.AvatarId);
                        fullPath = String.Empty;
                    }
                   
                    
                    dialog.AvatarId = fullPath;
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
