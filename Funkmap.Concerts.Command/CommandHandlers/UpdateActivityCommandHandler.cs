using System.Threading.Tasks;
using Funkmap.Common.Cqrs.Abstract;
using Funkmap.Concerts.Command.Commands;
using Funkmap.Concerts.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Funkmap.Concerts.Command.CommandHandlers
{
    public class UpdateActivityCommandHandler : ICommandHandler<UpdateActivityCommand>
    {
        private readonly IMongoCollection<ConcertEntity> _collection;

        public UpdateActivityCommandHandler(IMongoCollection<ConcertEntity> collection)
        {
            _collection = collection;
        }

        public async Task Execute(UpdateActivityCommand command)
        {
            var update = Builders<ConcertEntity>.Update.Set(x => x.IsActive, command.IsActive);
            await _collection.UpdateOneAsync(x => x.Id == new ObjectId(command.ConcertId), update);
        }
    }
}
