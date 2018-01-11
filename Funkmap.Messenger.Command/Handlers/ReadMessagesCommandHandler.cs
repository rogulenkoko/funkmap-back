using System;
using System.Threading.Tasks;
using Funkmap.Common.Cqrs.Abstract;
using Funkmap.Common.Logger;
using Funkmap.Messenger.Command.Commands;
using Funkmap.Messenger.Command.Repositories;
using Funkmap.Messenger.Events.Messages;

namespace Funkmap.Messenger.Command.Handlers
{
    internal class ReadMessagesCommandHandler : ICommandHandler<ReadMessagesCommand>
    {
        private readonly IMessengerCommandRepository _messengerRepository;
        private readonly IFunkmapLogger<SaveMessageCommandHandler> _logger;
        private readonly IEventBus _eventBus;

        public ReadMessagesCommandHandler(IMessengerCommandRepository messengerRepository, IFunkmapLogger<SaveMessageCommandHandler> logger, IEventBus eventBus)
        {
            _messengerRepository = messengerRepository;
            _logger = logger;
            _eventBus = eventBus;
        }

        public async Task Execute(ReadMessagesCommand command)
        {
            if (String.IsNullOrEmpty(command.DialogId) || String.IsNullOrEmpty(command.UserLogin) || command.ReadTime == DateTime.MinValue)
            {
                _logger.Info("Command validation failed");
                return;
            }

            try
            {
                await _messengerRepository.MakeDialogMessagesRead(command.DialogId, command.UserLogin, command.ReadTime);

                var dialogMembers = await _messengerRepository.GetDialogMembers(command.DialogId);
                await _eventBus.PublishAsync(new MessagesReadEvent()
                {
                    DialogId = command.DialogId,
                    ReadTime = command.ReadTime,
                    UserLogin = command.UserLogin,
                    DialogMembers = dialogMembers
                });
            }
            catch (Exception e)
            {
                _logger.Error(e, $"Read {command.DialogId} dialog messages failed");
            }
        }
    }
}
