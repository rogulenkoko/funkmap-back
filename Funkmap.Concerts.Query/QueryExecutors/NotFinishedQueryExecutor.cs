using System.Linq;
using System.Threading.Tasks;
using Funkmap.Common.Cqrs.Abstract;
using Funkmap.Concerts.Entities;
using Funkmap.Concerts.Query.Queries;
using Funkmap.Concerts.Query.Responses;
using MongoDB.Driver;

namespace Funkmap.Concerts.Query.QueryExecutors
{
    public class NotFinishedQueryExecutor : IQueryExecutor<NotFinishedQuery, NotFinishedConcertResponse>
    {
        private readonly IMongoCollection<ConcertEntity> _collection;

        public NotFinishedQueryExecutor(IMongoCollection<ConcertEntity> collection)
        {
            _collection = collection;
        }

        public async Task<NotFinishedConcertResponse> Execute(NotFinishedQuery query)
        {
            var filter = Builders<ConcertEntity>.Filter.Eq(x => x.IsFinished, false);
            var projection = Builders<ConcertEntity>.Projection.Include(x => x.Id)
                .Include(x => x.PeriodEndUtc)
                .Include(x => x.PeriodBeginUtc);

            var result = await _collection.Find(filter).Project<ConcertEntity>(projection).ToListAsync();

            var notFinished = result.Select(x => new NotFinishedConcert()
            {
                ConcertId = x.Id.ToString(),
                PeriodEndUtc = x.PeriodEndUtc,
                PeriodBeginUtc = x.PeriodBeginUtc
            }).ToList();

            var response = new NotFinishedConcertResponse(notFinished);

            return response;
        }
    }
}
