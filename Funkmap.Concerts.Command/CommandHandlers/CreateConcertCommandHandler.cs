﻿using System;
using System.IO;
using System.Threading.Tasks;
using Funkmap.Common.Cqrs.Abstract;
using Funkmap.Common.Logger;
using Funkmap.Concerts.Command.Commands;
using Funkmap.Concerts.Entities;
using Funkmap.Concerts.Events;
using MongoDB.Driver;

namespace Funkmap.Concerts.Command.CommandHandlers
{
    internal class CreateConcertCommandHandler : ICommandHandler<CreateConcertCommand>
    {
        private readonly IEventBus _eventBus;

        private readonly ICommandBus _commandBus;

        private readonly IMongoCollection<ConcertEntity> _collection;

        private readonly IFunkmapLogger<CreateConcertCommandHandler> _logger;

        public CreateConcertCommandHandler(IEventBus eventBus, 
                                           ICommandBus commandBus, 
                                           IMongoCollection<ConcertEntity> collection, 
                                           IFunkmapLogger<CreateConcertCommandHandler> logger)
        {
            _eventBus = eventBus;
            _commandBus = commandBus;
            _collection = collection;
            _logger = logger;
        }

        public async Task Execute(CreateConcertCommand command)
        {
            try
            {
                if (String.IsNullOrEmpty(command.Name) || String.IsNullOrEmpty(command.CreatorLogin)
                    || command.DateUtc == DateTime.MinValue)
                {
                    throw new InvalidDataException();
                }

                if (command.PeriodBeginUtc >= command.PeriodEndUtc)
                {
                    throw new InvalidDataException("Invalid period scope");
                }

                if (!(command.PeriodBeginUtc <= command.DateUtc && command.PeriodEndUtc >= command.DateUtc))
                {
                    throw new InvalidDataException("Invalid concert date");
                }

                var isActive = command.PeriodBeginUtc >= DateTime.UtcNow;

                var entity = new ConcertEntity()
                {
                    Name = command.Name,
                    CreatorLogin = command.CreatorLogin,
                    DateUtc = command.DateUtc,
                    Description = command.Description,
                    Participants = command.Participants,
                    PeriodBeginUtc = command.PeriodBeginUtc,
                    PeriodEndUtc = command.PeriodEndUtc,
                    CreationDateUtc = DateTime.UtcNow,
                    IsActive = isActive
                };

                await _collection.InsertOneAsync(entity);

                var afficheCommand = new UpdateAfficheCommand()
                {
                    User = command.CreatorLogin,
                    ConcertId = entity.Id.ToString(),
                    Data = command.AfficheBytes
                };

                _commandBus.Execute(afficheCommand);

                _eventBus.PublishAsync(new ConcertCreatedEvent(entity));
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
