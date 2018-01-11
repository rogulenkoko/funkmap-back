using System;
using System.IO;
using System.Threading.Tasks;
using Funkmap.Common.Cqrs.Abstract;
using Funkmap.Common.Logger;
using Funkmap.Messenger.Command.Commands;
using Funkmap.Messenger.Command.Repositories;
using Funkmap.Messenger.Events.Messages;

namespace Funkmap.Messenger.Command.CommandHandlers
{
    internal class ReadMessagesCommandHandler : ICommandHandler<ReadMessagesCommand>
    {
        private readonly IMessengerCommandRepository _messengerRepository;
        private readonly IFunkmapLogger<ReadMessagesCommandHandler> _logger;
        private readonly IEventBus _eventBus;

        public ReadMessagesCommandHandler(IMessengerCommandRepository messengerRepository, IFunkmapLogger<ReadMessagesCommandHandler> logger, IEventBus eventBus)
        {
            _messengerRepository = messengerRepository;
            _logger = logger;
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
                _logger.Error(ex, "Validation failed");
            }
            catch (Exception e)
            {
                _logger.Error(e, $"Read {command.DialogId} dialog messages failed");
            }
        }
    }
}
