using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Autofac.Features.AttributeFilters;
using Funkmap.Common.Abstract;
using Funkmap.Cqrs.Abstract;
using Funkmap.Feedback.Command.Commands;
using Funkmap.Feedback.Domain;
using Funkmap.Feedback.Entities;
using Funkmap.Logger;
using MongoDB.Driver;

namespace Funkmap.Feedback.Command.CommandHandler
{
    public class FeedbackCommandHandler : ICommandHandler<FeedbackCommand>
    {
        private readonly IMongoCollection<FeedbackEntity> _collection;
        private readonly IFileStorage _fileStorage;
        private readonly IFunkmapLogger<FeedbackCommandHandler> _logger;
        private readonly IEventBus _eventBus;

        public FeedbackCommandHandler(IMongoCollection<FeedbackEntity> collection, 
                                      IFunkmapLogger<FeedbackCommandHandler> logger, 
                                      IEventBus eventBus,
                                      [KeyFilter(FeedbackCollectionNameProvider.FeedbackStorage)]
                                      IFileStorage fileStorage)
        {
            _collection = collection;
            _logger = logger;
            _eventBus = eventBus;
            _fileStorage = fileStorage;
        }

        public async Task Execute(FeedbackCommand command)
        {
            try
            {
                if (string.IsNullOrEmpty(command?.Item?.Message))
                {
                    throw new InvalidDataException("Empty message");
                }

                var content = new List<FeedbackContentEntity>();

                if (command.Item.Content != null)
                {
                    foreach (var contentItem in command.Item.Content)
                    {
                        var name = $"{DateTime.UtcNow:yyyyMMddhhmmss}_{contentItem.Name}";
                        var url = await _fileStorage.UploadFromBytesAsync(name, contentItem.Data);
                        contentItem.DataUrl = url;
                        content.Add(new FeedbackContentEntity {Name = name, DataUrl = url});
                    }
                }

                var entity = new FeedbackEntity
                {
                    FeedbackType = command.Item.FeedbackType,
                    Message = command?.Item.Message,
                    Content = content,
                    Created = DateTime.UtcNow
                };
                
                await _collection.InsertOneAsync(entity);
                await _eventBus.PublishAsync(new FeedbackSavedEvent(command.Item));
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
