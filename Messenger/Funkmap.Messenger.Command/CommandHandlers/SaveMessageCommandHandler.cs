using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Funkmap.Common.Cqrs.Abstract;
using Funkmap.Common.Logger;
using Funkmap.Messenger.Command.Abstract;
using Funkmap.Messenger.Command.Commands;
using Funkmap.Messenger.Entities;
using Funkmap.Messenger.Events;
using Funkmap.Messenger.Events.Messages;
using MongoDB.Bson;

namespace Funkmap.Messenger.Command.CommandHandlers
{
    internal class SaveMessageCommandHandler : ICommandHandler<SaveMessageCommand>
    {
        private readonly IMessengerCommandRepository _messengerRepository;
        private readonly IEventBus _eventBus;

        public SaveMessageCommandHandler(IMessengerCommandRepository messengerRepository, IEventBus eventBus, IFunkmapLogger<SaveMessageCommandHandler> logger)
        {
            _messengerRepository = messengerRepository;
            _eventBus = eventBus;
        }

        public async Task Execute(SaveMessageCommand command)
        {
            try
            {
                if (String.IsNullOrEmpty(command.DialogId) || command.DialogId == ObjectId.Empty.ToString())
                {
                    throw new InvalidDataException("Invalid new message's dialog id");
                }

                if (String.IsNullOrEmpty(command.Sender))
                {
                    throw new InvalidDataException("Invalid new message's sender");
                }

                if (String.IsNullOrEmpty(command.Text) && (command.Content == null || command.Content.Count == 0))
                {
                    throw new InvalidDataException("Invalid new message's content");
                }

                var dialogParticipants = await _messengerRepository.GetDialogMembersAsync(command.DialogId);

                if (!dialogParticipants.Contains(command.Sender))
                {
                    throw new InvalidDataException($"{command.Sender} is not member of the dialog with id {command}");
                }
                var participants = dialogParticipants.Where(x => x != command.Sender).ToList();

                var isRead = command.UsersWithOpenedCurrentDialog != null && command.UsersWithOpenedCurrentDialog.Any(x => x != command.Sender);

                var messsageParticipants = command.UsersWithOpenedCurrentDialog == null
                    ? participants
                    : participants.Except(command.UsersWithOpenedCurrentDialog).ToList();

                var message = new MessageEntity()
                {
                    Content = command.Content,
                    DateTimeUtc = DateTime.UtcNow,
                    Sender = command.Sender,
                    DialogId = new ObjectId(command.DialogId),
                    IsRead = isRead,
                    Text = command.Text,
                    ToParticipants = messsageParticipants,
                    MessageType = command.MessageType
                };

                await _messengerRepository.AddMessageAsync(message);
                await _eventBus.PublishAsync(new MessageSavedCompleteEvent()
                {
                    Success = true,
                    Message = message,
                    DialogParticipants = dialogParticipants.ToList()
                });
            }
            catch (InvalidDataException ex)
            {
                var error = $"{nameof(SaveMessageCommand)} validation failed.";
                await _eventBus.PublishAsync(new MessageSavedFailEvent() {Error = error});
                await _eventBus.PublishAsync(new MessengerCommandFailedEvent()
                {
                    Error = error,
                    ExceptionMessage = ex.Message,
                    Sender = command.Sender
                });
            }
            catch (Exception e)
            {
                var error = "Message saving failed.";
                await _eventBus.PublishAsync(new MessageSavedFailEvent());
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
