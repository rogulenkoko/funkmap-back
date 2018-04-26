using System;
using System.IO;
using System.Threading.Tasks;
using Autofac.Features.AttributeFilters;
using Funkmap.Common.Abstract;
using Funkmap.Common.Cqrs.Abstract;
using Funkmap.Common.Tools;
using Funkmap.Messenger.Command.Commands;
using Funkmap.Messenger.Contracts.Events;
using Funkmap.Messenger.Contracts.Events.Messages;
using Funkmap.Messenger.Entities;

namespace Funkmap.Messenger.Command.CommandHandlers
{
    internal class StartUploadContentCommandHandler : ICommandHandler<StartUploadContentCommand>
    {
        private readonly IEventBus _eventBus;
        private readonly IFileStorage _fileStorage;

        public StartUploadContentCommandHandler(IEventBus eventBus,
                                                [KeyFilter(MessengerCollectionNameProvider.MessengerStorage)]
                                                IFileStorage fileStorage)
        {
            _eventBus = eventBus;
            _fileStorage = fileStorage;
        }

        public async Task Execute(StartUploadContentCommand command)
        {
            try
            {
                if (command.FileBytes == null || command.FileBytes.Length == 0 || String.IsNullOrEmpty(command.FileName))
                {
                    throw new InvalidDataException("Invalid content item");
                }

                if (command.ContentType == ContentType.Image)
                {
                    var maxWidth = 700;
                    command.FileBytes = FunkmapImageProcessor.MinifyImageWithMaxWidth(command.FileBytes, maxWidth);
                }

                var date = DateTime.UtcNow.ToString("yyyyMMddhhmmss");
                var fileId = await _fileStorage.UploadFromBytesAsync($"{date}_{command.FileName}", command.FileBytes);

                await _eventBus.PublishAsync(new ContentUploadedEvent(command.ContentType, command.FileName, fileId, command.Sender));
            }
            catch (InvalidDataException ex)
            {
                var error = $"{nameof(StartUploadContentCommand)} validation failed.";
                await _eventBus.PublishAsync(new MessengerCommandFailedEvent()
                {
                    Error = error,
                    ExceptionMessage = ex.Message,
                    Sender = command.Sender
                });
            }
            catch (Exception e)
            {
                var error = "Content uploading failed.";
                await _eventBus.PublishAsync(new MessengerCommandFailedEvent()
                {
                    Error = error,
                    ExceptionMessage = e.Message,
                    Sender = command.Sender
                });
            }
        }
    }
}

