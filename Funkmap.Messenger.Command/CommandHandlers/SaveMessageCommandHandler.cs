using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Funkmap.Common.Cqrs.Abstract;
using Funkmap.Common.Logger;
using Funkmap.Common.Tools;
using Funkmap.Messenger.Command.Commands;
using Funkmap.Messenger.Command.Repositories;
using Funkmap.Messenger.Entities;
using Funkmap.Messenger.Events.Messages;
using MongoDB.Bson;

namespace Funkmap.Messenger.Command.CommandHandlers
{
    internal class SaveMessageCommandHandler : ICommandHandler<SaveMessageCommand>
    {
        private readonly IMessengerCommandRepository _messengerRepository;
        private readonly IFunkmapLogger<SaveMessageCommandHandler> _logger;
        private readonly IEventBus _eventBus;

        public SaveMessageCommandHandler(IMessengerCommandRepository messengerRepository, IEventBus eventBus, IFunkmapLogger<SaveMessageCommandHandler> logger)
        {
            _messengerRepository = messengerRepository;
            _logger = logger;
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

                if (!dialogParticipants.Contains(command.Sender) && command.Sender != FunkmapConstants.FunkmalAdminUser)
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
                    ToParticipants = messsageParticipants
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
                var error = "Message validation failed";
                await _eventBus.PublishAsync(new MessageSavedFailEvent() {Error = error});
                _logger.Error(ex, error);
            }
            catch (Exception e)
            {
                await _eventBus.PublishAsync(new MessageSavedFailEvent());
                _logger.Error(e);
            }
        }
    }
}
