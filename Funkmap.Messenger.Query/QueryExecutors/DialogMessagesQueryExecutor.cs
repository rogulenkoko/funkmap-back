using System;
using System.Collections.Generic;
using System.Linq;
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
    internal class DialogMessagesQueryExecutor : IQueryExecutor<DialogMessagesQuery, DialogMessagesResponse>
    {
        private readonly IMongoCollection<MessageEntity> _collection;
        private readonly IMongoCollection<DialogEntity> _dialogsCollection;
        private readonly IFunkmapLogger<DialogMessagesQueryExecutor> _logger;

        public DialogMessagesQueryExecutor(IMongoCollection<MessageEntity> collection, IMongoCollection<DialogEntity> dialogsCollection, IFunkmapLogger<DialogMessagesQueryExecutor> logger)
        {
            _collection = collection;
            _dialogsCollection = dialogsCollection;
            _logger = logger;
        }


        public async Task<DialogMessagesResponse> Execute(DialogMessagesQuery query)
        {
            try
            {
                if (String.IsNullOrEmpty(query.UserLogin) || String.IsNullOrEmpty(query.DialogId))
                {
                    throw new ArgumentException(nameof(query));
                }

                //validation

                var filter = Builders<DialogEntity>.Filter.Eq(x => x.Id, new ObjectId(query.DialogId));
                var projection = Builders<DialogEntity>.Projection.Include(x => x.Id).Include(x=>x.Participants);
                var existingDialog = await _dialogsCollection.Find(filter).Project<DialogEntity>(projection).SingleOrDefaultAsync();

                if (existingDialog == null)
                {
                    throw new InvalidOperationException($"Dialog {query.DialogId} is not exists");
                }

                if (!existingDialog.Participants.Contains(query.UserLogin))
                {
                    throw new InvalidOperationException($"User {query.UserLogin} is not a member of dialog {query.DialogId}");
                }

                //query

                var dialogFilter = Builders<MessageEntity>.Filter.Eq(x => x.DialogId, new ObjectId(query.DialogId));
                var messageProjection = Builders<MessageEntity>.Projection
                    .Exclude(x => x.ToParticipants);

                var sort = Builders<MessageEntity>.Sort.Descending(x => x.Id);

                ICollection<MessageEntity> messages = await _collection.Find(dialogFilter).Project<MessageEntity>(messageProjection)
                    .Sort(sort)
                    .Skip(query.Skip)
                    .Limit(query.Take)
                    .ToListAsync();

                if (messages == null || messages.Count == 0)
                {
                    return new DialogMessagesResponse(true, new List<Message>());
                }

                var orderedMessages = messages.Reverse();

                var response = new DialogMessagesResponse(true, orderedMessages.Select(x=> new Message()
                {
                    DialogId = x.DialogId.ToString(),
                    Sender = x.Sender,
                    Text = x.Text,
                    DateTimeUtc = x.DateTimeUtc,
                    IsNew = !x.IsRead,
                    MessageType = x.MessageType,
                    Content = x.Content.Select(c=> new ContentItem()
                    {
                        ContentType = c.ContentType,
                        FileName = c.FileName,
                        Size = c.Size,
                        FileId = c.FileId
                    }).ToList()
                }).ToList());

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
