using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Funkmap.Common.Cqrs.Abstract;
using Funkmap.Common.Logger;
using Funkmap.Messenger.Entities;
using Funkmap.Messenger.Query.Queries;
using Funkmap.Messenger.Query.Responses;
using MongoDB.Driver;

namespace Funkmap.Messenger.Query.QueryExecutors
{
    internal class DialogsNewMessagesCountQueryExecutor : IQueryExecutor<DialogsNewMessagesCountQuery, DialogsNewMessagesCountResponse>
    {
        private readonly IMongoCollection<MessageEntity> _collection;
        private readonly IMongoCollection<DialogEntity> _dialogsCollection;
        private readonly IFunkmapLogger<DialogsNewMessagesCountQueryExecutor> _logger;

        public DialogsNewMessagesCountQueryExecutor(IMongoCollection<MessageEntity> collection, 
                                                    IMongoCollection<DialogEntity> dialogsCollection,
                                                    IFunkmapLogger<DialogsNewMessagesCountQueryExecutor> logger)
        {
            _collection = collection;
            _logger = logger;
            _dialogsCollection = dialogsCollection;
        }

        public async Task<DialogsNewMessagesCountResponse> Execute(DialogsNewMessagesCountQuery query)
        {
            try
            {

                var filter = Builders<DialogEntity>.Filter.AnyEq(x => x.Participants, query.UserLogin);
                var projection = Builders<DialogEntity>.Projection.Include(x => x.Id);

                var dialogs = await _dialogsCollection.Find(filter).Project<DialogEntity>(projection).ToListAsync();

                var newMessagesFilter = Builders<MessageEntity>.Filter.AnyEq(x => x.ToParticipants, query.UserLogin)
                                        & Builders<MessageEntity>.Filter.In(x => x.DialogId, dialogs.Select(x => x.Id));

                ICollection<DialogsNewMessagesCountResultEntity> countResultEntities = await _collection.Aggregate()
                    .Match(newMessagesFilter)
                    .Group(x => x.DialogId, y => new DialogsNewMessagesCountResultEntity()
                    {
                        DialogId = y.Key,
                        NewMessagesCount = y.Count()
                    })
                    .ToListAsync();

                var countResult = countResultEntities.Select(x => new DialogsNewMessagesCountResult()
                {
                    DialogId = x.DialogId.ToString(),
                    NewMessagesCount = x.NewMessagesCount
                }).ToList();

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
