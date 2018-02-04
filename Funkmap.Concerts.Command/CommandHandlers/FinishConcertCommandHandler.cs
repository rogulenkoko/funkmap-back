using System.Threading.Tasks;
using Funkmap.Common.Cqrs.Abstract;
using Funkmap.Concerts.Command.Commands;
using Funkmap.Concerts.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Funkmap.Concerts.Command.CommandHandlers
{
    public class FinishConcertCommandHandler : ICommandHandler<FinishConcertCommand>
    {
        private readonly IMongoCollection<ConcertEntity> _collection;

        public FinishConcertCommandHandler(IMongoCollection<ConcertEntity> collection)
        {
            _collection = collection;
        }

        public async Task Execute(FinishConcertCommand command)
        {
            var update = Builders<ConcertEntity>.Update.Set(x => x.IsFinished, true).Set(x => x.IsActive, false);
            await _collection.UpdateOneAsync(x => x.Id == new ObjectId(command.ConcertId), update);
        }
    }
}
