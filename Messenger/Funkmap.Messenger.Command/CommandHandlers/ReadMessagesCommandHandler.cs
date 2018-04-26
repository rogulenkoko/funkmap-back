using System;
using System.IO;
using System.Threading.Tasks;
using Funkmap.Common.Cqrs.Abstract;
using Funkmap.Messenger.Command.Abstract;
using Funkmap.Messenger.Command.Commands;
using Funkmap.Messenger.Contracts.Events;
using Funkmap.Messenger.Contracts.Events.Messages;

namespace Funkmap.Messenger.Command.CommandHandlers
{
    internal class ReadMessagesCommandHandler : ICommandHandler<ReadMessagesCommand>
    {
        private readonly IMessengerCommandRepository _messengerRepository;
        private readonly IEventBus _eventBus;

        public ReadMessagesCommandHandler(IMessengerCommandRepository messengerRepository, IEventBus eventBus)
        {
            _messengerRepository = messengerRepository;
            _eventBus = eventBus;
        }

        public async Task Execute(ReadMessagesCommand command)
        {
            try
            {
                if (String.IsNullOrEmpty(command.DialogId) || String.IsNullOrEmpty(command.UserLogin) || command.ReadTime == DateTime.MinValue)
                {
                    throw new InvalidDataException("Command validation failed");
                }


                await _messengerRepository.MakeDialogMessagesReadAsync(command.DialogId, command.UserLogin, command.ReadTime);

                var dialogMembers = await _messengerRepository.GetDialogMembersAsync(command.DialogId);
                await _eventBus.PublishAsync(new MessagesReadEvent()
                {
                    DialogId = command.DialogId,
                    ReadTime = command.ReadTime,
                    UserLogin = command.UserLogin,
                    DialogMembers = dialogMembers
                });
            }
            catch (InvalidDataException ex)
            {
                var error = $"{nameof(ReadMessagesCommand)} validation failed.";
                await _eventBus.PublishAsync(new MessengerCommandFailedEvent()
                {
                    Error = error,
                    ExceptionMessage = ex.Message,
                    Sender = command.UserLogin
                });
            }
            catch (Exception e)
            {
                var error = $"Read {command.DialogId} dialog messages failed.";
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
