using System;
using System.IO;
using System.Threading.Tasks;
using Funkmap.Common.Abstract;
using Funkmap.Common.Cqrs.Abstract;
using Funkmap.Common.Logger;
using Funkmap.Concerts.Command.Commands;
using Funkmap.Concerts.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Funkmap.Concerts.Command.CommandHandlers
{
    internal class UpdateAfficheCommandHandler : ICommandHandler<UpdateAfficheCommand>
    {

        private readonly IFileStorage _fileStorage;

        private readonly IMongoCollection<ConcertEntity> _collection;

        private readonly IFunkmapLogger<UpdateAfficheCommandHandler> _logger;


        public UpdateAfficheCommandHandler(IFileStorage fileStorage, IMongoCollection<ConcertEntity> collection, IFunkmapLogger<UpdateAfficheCommandHandler> logger)
        {
            _fileStorage = fileStorage;
            _collection = collection;
            _logger = logger;
        }

        public async Task Execute(UpdateAfficheCommand command)
        {
            try
            {
                var concert = await _collection.Find(x => x.Id == new ObjectId(command.ConcertId)).SingleOrDefaultAsync();

                if (concert == null)
                {
                    throw new InvalidDataException($"Concert {command.ConcertId} not found");
                }

                if (concert.CreatorLogin != command.User)
                {
                    throw new InvalidDataException($"User {command.User} cant modify concert {command.ConcertId}");
                }
                
                String url;

                if (command.Data == null || command.Data.Length == 0)
                {
                    url = String.Empty;
                }
                else
                {
                    var fileName = $"{concert.Id}avatar.png";
                    url = await _fileStorage.UploadFromBytesAsync(fileName, command.Data);
                }

                var update = Builders<ConcertEntity>.Update.Set(x => x.AfficheUrl, url);

                await _collection.UpdateOneAsync(x => x.Id == concert.Id, update);

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
