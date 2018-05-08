using System;
using System.Threading.Tasks;
using Funkmap.Common.Cqrs.Abstract;
using Funkmap.Common.Logger;
using Funkmap.Messenger.Entities;
using Funkmap.Messenger.Query.Queries;
using Funkmap.Messenger.Query.Responses;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Funkmap.Messenger.Query.QueryExecutors
{
    internal class DialogAvatarInfoQueryExecutor : IQueryExecutor<DialogAvatarInfoQuery, DialogAvatarInfoResponse>
    {
        private readonly IMongoCollection<DialogEntity> _dialogsCollection;
        private readonly IFunkmapLogger<DialogAvatarInfoQueryExecutor> _logger;

        public DialogAvatarInfoQueryExecutor(IMongoCollection<DialogEntity> dialogsCollection, IFunkmapLogger<DialogAvatarInfoQueryExecutor> logger)
        {
            _dialogsCollection = dialogsCollection;
            _logger = logger;
        }

        public async Task<DialogAvatarInfoResponse> Execute(DialogAvatarInfoQuery query)
        {
            try
            {
                var projection = Builders<DialogEntity>.Projection.Include(x => x.Avatar);
                var filter = Builders<DialogEntity>.Filter.Eq(x => x.Id, new ObjectId(query.DialogId));
                var dialog = await _dialogsCollection.Find(filter).Project<DialogEntity>(projection).SingleOrDefaultAsync();

                var response = new DialogAvatarInfoResponse(true, dialog.Avatar?.Image?.AsByteArray, query.DialogId);

                return response;
            }
            catch (Exception e)
            {
                _logger.Error(e, "Query execution failed.");
                throw;
            }
        }
    }
}
