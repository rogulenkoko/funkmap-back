using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Funkmap.Common.Cqrs.Abstract;
using Funkmap.Common.Logger;
using Funkmap.Messenger.Contracts;
using Funkmap.Messenger.Entities;
using Funkmap.Messenger.Entities.Objects;
using Funkmap.Messenger.Query.Queries;
using Funkmap.Messenger.Query.Responses;
using MongoDB.Driver;

namespace Funkmap.Messenger.Query.QueryExecutors
{
    internal class UserDialogsQueryExecutor : IQueryExecutor<UserDialogsQuery, UserDialogsResponse>
    {
        private readonly IMongoCollection<DialogEntity> _collection;
        private readonly IMongoCollection<MessageEntity> _messagesCollection;
        private readonly IFunkmapLogger<UserDialogsQueryExecutor> _logger;

        public UserDialogsQueryExecutor(IMongoCollection<DialogEntity> collection,
                                        IMongoCollection<MessageEntity> messagesCollection,
                                        IFunkmapLogger<UserDialogsQueryExecutor> logger)
        {
            _collection = collection;
            _messagesCollection = messagesCollection;
            _logger = logger;
        }

        public async Task<UserDialogsResponse> Execute(UserDialogsQuery query)
        {
            //db.dialogs.aggregate([
            //{$match: { _id: ObjectId("5adb9b29b5a0ea1a186895a9")} },
            //{
            //    $lookup:
            //    {
            //    from: "messages",
            //        localField: "_id",
            //        foreignField: "d",
            //            as: "mes"
            //    }
            //},
            //{$project: { _id: 1, mes: "$mes" } }
            //])


            try
            {
                var filter = Builders<DialogEntity>.Filter.AnyEq(x => x.Participants, query.UserLogin);

                var sortFilter = Builders<DialogEntity>.Sort.Descending(x => x.LastMessageDate);
                
                List<DialogEntity> dialogs = await _collection.Find(filter).Sort(sortFilter).ToListAsync();

                var response = new UserDialogsResponse(true, dialogs.Where(x=>x.LastMessage != null).Select(x => new DialogWithLastMessage()
                {
                    DialogId = x.Id.ToString(),
                    Name = x.Name,
                    Participants = x.Participants,
                    AvatarId = x.AvatarId,
                    CreatorLogin = x.CreatorLogin,
                    LastMessage = new Message()
                    {
                        Id = x.Id.ToString(),
                        Text = x.LastMessage.Text,
                        DialogId = x.LastMessage.DialogId.ToString(),
                        Sender = x.LastMessage.Sender,
                        DateTimeUtc = x.LastMessage.DateTimeUtc,
                        IsNew = !x.LastMessage.IsRead,
                        MessageType = x.LastMessage.MessageType,
                        Content = x.LastMessage.Content.Select(c => new ContentItem()
                        {
                            ContentType = c.ContentType,
                            FileName = c.FileName,
                            Size = c.Size,
                            FileId = c.FileId
                        }).ToList()
                    },
                    DialogType = x.DialogType
                }).ToList());

                var dialogIds = dialogs.Select(x => x.Id);

                var newMessagesFilter = Builders<MessageEntity>.Filter.AnyEq(x => x.ToParticipants, query.UserLogin)
                                        & Builders<MessageEntity>.Filter.In(x => x.DialogId, dialogIds);

                var countResult = await _messagesCollection.Aggregate()
                    .Match(newMessagesFilter)
                    .Group(x => x.DialogId, y => new DialogsNewMessagesCountResultEntity()
                    {
                        DialogId = y.Key,
                        NewMessagesCount = y.Count()
                    })
                    .ToListAsync();

                var countResultDictionary = countResult.ToDictionary(x => x.DialogId.ToString(), y => y.NewMessagesCount);

                foreach (var dialog in response.Dialogs)
                {
                    if(!countResultDictionary.ContainsKey(dialog.DialogId)) continue;

                    dialog.NewMessagesCount = countResultDictionary[dialog.DialogId];
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
