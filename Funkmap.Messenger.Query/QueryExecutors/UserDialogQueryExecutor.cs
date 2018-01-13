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
    internal class UserDialogQueryExecutor : IQueryExecutor<UserDialogQuery, UserDialogResponse>
    {
        private readonly IMongoCollection<DialogEntity> _collection;
        private readonly IMongoCollection<MessageEntity> _messagesCollection;
        private readonly IFunkmapLogger<UserDialogQueryExecutor> _logger;

        public UserDialogQueryExecutor(IMongoCollection<DialogEntity> collection,
            IMongoCollection<MessageEntity> messagesCollection,
            IFunkmapLogger<UserDialogQueryExecutor> logger)
        {
            _collection = collection;
            _messagesCollection = messagesCollection;
            _logger = logger;
        }


        public async Task<UserDialogResponse> Execute(UserDialogQuery query)
        {
            try
            {
                var filter = Builders<DialogEntity>.Filter.AnyEq(x => x.Participants, query.UserLogin)
                            & Builders<DialogEntity>.Filter.Eq(x=>x.Id, new ObjectId(query.DialogId));

                var dialog = await _collection.Aggregate()
                    .Match(filter)
                    .Lookup<DialogEntity, MessageEntity, DialogLookup>(_messagesCollection, x => x.Id, x => x.DialogId, result => result.LastMessages)
                    .Project(x => new DialogEntity()
                    {
                        Id = x.Id,
                        LastMessage = x.LastMessages.Last(),
                        Name = x.Name,
                        Participants = x.Participants,
                        LastMessageDate = x.LastMessageDate,
                        CreatorLogin = x.CreatorLogin
                    })
                    .SingleOrDefaultAsync();

                if(dialog == null) return new UserDialogResponse(true, null);

                var response = new UserDialogResponse(true,  new DialogWithLastMessage()
                {
                    DialogId = dialog.Id.ToString(),
                    Name = dialog.Name,
                    Participants = dialog.Participants,
                    CreatorLogin = dialog.CreatorLogin,
                    LastMessage = new Message()
                    {
                        Text = dialog.LastMessage.Text,
                        DialogId = dialog.LastMessage.DialogId.ToString(),
                        Sender = dialog.LastMessage.Sender,
                        DateTimeUtc = dialog.LastMessage.DateTimeUtc,
                        IsNew = !dialog.LastMessage.IsRead
                    }
                });

                var newMessagesFilter = Builders<MessageEntity>.Filter.AnyEq(x => x.ToParticipants, query.UserLogin)
                                        & Builders<MessageEntity>.Filter.Eq(x => x.DialogId, dialog.Id);

                var countResult = await _messagesCollection.Aggregate()
                    .Match(newMessagesFilter)
                    .Group(x => x.DialogId, y => new DialogsNewMessagesCountResultEntity()
                    {
                        DialogId = y.Key,
                        NewMessagesCount = y.Count()
                    })
                    .SingleOrDefaultAsync();

                if (countResult != null)
                {
                    response.Dialog.NewMessagesCount = countResult.NewMessagesCount;
                }
                
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
