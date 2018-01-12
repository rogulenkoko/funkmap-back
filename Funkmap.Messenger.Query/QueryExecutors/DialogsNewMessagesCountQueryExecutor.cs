using System;
using System.Linq;
using System.Threading.Tasks;
using Funkmap.Common.Cqrs.Abstract;
using Funkmap.Common.Logger;
using Funkmap.Messenger.Entities;
using Funkmap.Messenger.Entities.Objects;
using Funkmap.Messenger.Query.Queries;
using Funkmap.Messenger.Query.Responses;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Funkmap.Messenger.Query.QueryExecutors
{
    public class DialogsNewMessagesCountQueryExecutor : IQueryExecutor<DialogsNewMessagesCountQuery, DialogsNewMessagesCountResponse>
    {
        private readonly IMongoCollection<MessageEntity> _collection;
        private readonly IFunkmapLogger<DialogsNewMessagesCountQueryExecutor> _logger;

        public DialogsNewMessagesCountQueryExecutor(IMongoCollection<MessageEntity> collection, IFunkmapLogger<DialogsNewMessagesCountQueryExecutor> logger)
        {
            _collection = collection;
            _logger = logger;
        }

        public async Task<DialogsNewMessagesCountResponse> Execute(DialogsNewMessagesCountQuery query)
        {
            try
            {
                var newMessagesFilter = Builders<MessageEntity>.Filter.AnyEq(x => x.ToParticipants, query.UserLogin)
                                        & Builders<MessageEntity>.Filter.In(x => x.DialogId, query.DialogIds.Select(x => new ObjectId(x)));

                var countResult = await _collection.Aggregate()
                    .Match(newMessagesFilter)
                    .Group(x => x.DialogId, y => new DialogsNewMessagesCountResult()
                    {
                        DialogId = y.Key.ToString(),
                        NewMessagesCount = y.Count()
                    })
                    .ToListAsync();

                var response = new DialogsNewMessagesCountResponse(true, countResult);
                return response;
            }
            catch (Exception e)
            {
                _logger.Error(e, "Query execution failed");
                throw;
            }
        }
    }
}
