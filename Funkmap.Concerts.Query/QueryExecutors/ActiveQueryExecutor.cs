using System.Linq;
using System.Threading.Tasks;
using Funkmap.Common.Cqrs.Abstract;
using Funkmap.Concerts.Entities;
using Funkmap.Concerts.Query.Queries;
using Funkmap.Concerts.Query.Responses;
using MongoDB.Driver;

namespace Funkmap.Concerts.Query.QueryExecutors
{
    public class ActiveQueryExecutor : IQueryExecutor<ActiveQuery, ActiveResponse>
    {
        private readonly IMongoCollection<ConcertEntity> _collection;

        public ActiveQueryExecutor(IMongoCollection<ConcertEntity> collection)
        {
            _collection = collection;
        }

        public async Task<ActiveResponse> Execute(ActiveQuery query)
        {
            var projection = Builders<ConcertEntity>.Projection.Include(x => x.Id)
                .Include(x => x.Latitude)
                .Include(x => x.Longitude);

            var filter = Builders<ConcertEntity>.Filter.Eq(x => x.IsActive, true);

            var result = await _collection.Find(filter).Project<ConcertEntity>(projection).ToListAsync();

            var markers = result.Select(x => new ActiveConcertMarker() { Id = x.Id.ToString(), Longitude = x.Longitude, Latitude = x.Latitude }).ToList();

            var response = new ActiveResponse() { ConcertMarkers = markers };

            return response;
        }
    }
}
