﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Funkmap.Common.Cqrs.Abstract;
using Funkmap.Common.Logger;
using Funkmap.Messenger.Data.Objects;
using Funkmap.Messenger.Entities;
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
            //db.dialogs.aggregate(
            //    [{$match: { _id: ObjectId("5a304763208c10408cac622e")} },
            //{$lookup: { from: "messages", localField: "_id", foreignField: "d", as: "mes" } },
            //{$project: { _id: 1, message: { $arrayElemAt: ["$mes", 0] } } }
            //])

            try
            {
                var filter = Builders<DialogEntity>.Filter.AnyEq(x => x.Participants, query.UserLogin);

                var sortFilter = Builders<DialogEntity>.Sort.Descending(x => x.LastMessageDate);

                var dialogs = await _collection.Aggregate()
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
                    .Sort(sortFilter)
                    .ToListAsync();



                var response = new UserDialogsResponse(true, dialogs.Select(x => new DialogWithLastMessage()
                {
                    DialogId = x.Id.ToString(),
                    Name = x.Name,
                    Participants = x.Participants,
                    CreatorLogin = x.CreatorLogin,
                    LastMessage = x.LastMessage == null ? null : new Message()
                    {
                        Text = x.LastMessage.Text,
                        DialogId = x.LastMessage.DialogId.ToString(),
                        Sender = x.LastMessage.Sender,
                        DateTimeUtc = x.LastMessage.DateTimeUtc,
                        IsNew = !x.LastMessage.IsRead
                    }
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
