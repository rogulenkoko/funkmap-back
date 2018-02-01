using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Common.Cqrs.Abstract;
using Funkmap.Concerts.Command.Commands;
using Funkmap.Concerts.Entities;
using MongoDB.Driver;

namespace Funkmap.Concerts.Command.CommandHandlers
{
    internal class CreateConcertCommandHandler : ICommandHandler<CreateConcertCommand>
    {
        private IEventBus _eventBus;

        private readonly IMongoCollection<ConcertEntity> _collection;

        public async Task Execute(CreateConcertCommand command)
        {

            

            var entity = new ConcertEntity()
            {
                Name = command.Name,
                CreatorLogin = command.CreatorLogin,
                Date = command.Date,
                Description = command.Description,
                Participants = command.Participants,
                PeriodBegin = command.PeriodBegin,
                PeriodEnd = command.PeriodEnd
            };



        }
    }
}
