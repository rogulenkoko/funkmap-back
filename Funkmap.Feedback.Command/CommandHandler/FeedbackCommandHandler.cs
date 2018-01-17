using System;
using System.IO;
using System.Threading.Tasks;
using Funkmap.Common.Cqrs.Abstract;
using Funkmap.Common.Logger;
using Funkmap.Feedback.Command.Commands;
using Funkmap.Feedback.Entities;
using Funkmap.Feedback.Events;
using MongoDB.Driver;

namespace Funkmap.Feedback.Command.CommandHandler
{
    public class FeedbackCommandHandler : ICommandHandler<FeedbackCommand>
    {
        private readonly IMongoCollection<FeedbackEntity> _collection;
        private readonly IFunkmapLogger<FeedbackCommandHandler> _logger;
        private readonly IEventBus _eventBus;

        public FeedbackCommandHandler(IMongoCollection<FeedbackEntity> collection, IFunkmapLogger<FeedbackCommandHandler> logger, IEventBus eventBus)
        {
            _collection = collection;
            _logger = logger;
            _eventBus = eventBus;
        }

        public async Task Execute(FeedbackCommand command)
        {
            try
            {
                if (String.IsNullOrEmpty(command.Message))
                {
                    throw new InvalidDataException("Empty message");
                }

                var entity = new FeedbackEntity()
                {
                    FeedbackType = command.FeedbackType,
                    Message = command.Message
                };

                await _collection.InsertOneAsync(entity);
                await _eventBus.PublishAsync(new FeedbackSavedEvent(entity));

            }
            catch (InvalidDataException e)
            {
                _logger.Error(e, "Validation failed");
            }
            catch (Exception e)
            {
                _logger.Error(e);
            }
            
        }
    }
}
