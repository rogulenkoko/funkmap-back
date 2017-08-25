using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Common.Data.Mongo;
using Funkmap.Messenger.Data.Entities;
using Funkmap.Messenger.Data.Objects;
using Funkmap.Messenger.Data.Parameters;
using Funkmap.Messenger.Data.Repositories.Abstract;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace Funkmap.Messenger.Data.Repositories
{
    public class MessageRepository :IMessageRepository
    {
        private readonly IMongoCollection<MessageEntity> _collection;
        private readonly IGridFSBucket _gridFs;

        public MessageRepository(IMongoCollection<MessageEntity> collection, IGridFSBucket gridFs)
        {
            _collection = collection;
            _gridFs = gridFs;
        }

        public async Task AddMessage(MessageEntity message)
        {
            if (message.DialogId  == null || message.DialogId == ObjectId.Empty)
            {
                throw new ArgumentException(nameof(message.DialogId));
            }

            Parallel.ForEach(message.Content, item =>
            {
                var fileId = _gridFs.UploadFromBytes(item.FileName, item.FileBytes);
                item.FileId = fileId;
            });

            await _collection.InsertOneAsync(message);
        }

        public async Task<ICollection<MessageEntity>> GetLastDialogsMessages(string[] dialogIds)
        {
            //db.messages.aggregate([
            //{$match: { d: ObjectId("599f49c5208c1127f4f2dde4")} },
            //{$sort: { _id: -1} },
            //{$group: { _id: "$d",m: {$first: "$$ROOT"} } }
            //])


            if (dialogIds == null || dialogIds.Length == 0) throw new ArgumentException(nameof(dialogIds));

            var sort = Builders<MessageEntity>.Sort.Descending(x => x.Id);

            var dialogsFilter = Builders<MessageEntity>.Filter.In(x => x.DialogId, dialogIds.Select(x=>new ObjectId(x)));

            var messages = await _collection.Aggregate()
                .Match(dialogsFilter)
                .Sort(sort)
                .Group(x => x.DialogId, y => new LastDialogMessageResult()
                {
                    DialogId = y.Key,
                    LastMessage = new BsonDocument("$first", "$$ROOT") 
                }).ToListAsync();

            if(messages == null) return new List<MessageEntity>();

            return messages.Select(x => BsonSerializer.Deserialize<MessageEntity>(x.LastMessage)).ToList();
        }

        public async Task<ICollection<MessageEntity>> GetDialogMessagesAsync(DialogMessagesParameter parameter)
        {

            if (String.IsNullOrEmpty(parameter.UserLogin) || String.IsNullOrEmpty(parameter.DialogId))
            {
                throw new ArgumentException(nameof(parameter));
            }

            var dialogFilter = Builders<MessageEntity>.Filter.Eq(x => x.DialogId, new ObjectId(parameter.DialogId));
            var messageProjection = Builders<MessageEntity>.Projection
                .Exclude(x => x.ToParticipants);

            var sort = Builders<MessageEntity>.Sort.Descending(x => x.Id);

            ICollection<MessageEntity> messages = await _collection.Find(dialogFilter).Project<MessageEntity>(messageProjection)
                .Sort(sort)
                .Skip(parameter.Skip)
                .Limit(parameter.Take)
                .ToListAsync();
            if(messages == null || messages.Count == 0) return new List<MessageEntity>();

            //дата последнего сообщения
            DateTime lastMessageDate = messages.First().DateTimeUtc;

            var readFilter = Builders<MessageEntity>.Filter.AnyEq(x => x.ToParticipants, parameter.UserLogin)
                            & Builders<MessageEntity>.Filter.Lte(x=>x.DateTimeUtc, lastMessageDate)
                            & Builders<MessageEntity>.Filter.Ne(x=> x.Sender, parameter.UserLogin);

            var update = Builders<MessageEntity>.Update.Pull(x => x.ToParticipants, parameter.UserLogin).Set(x=>x.IsRead, true);

            await _collection.UpdateManyAsync(readFilter, update);
                
            return messages.Reverse().ToList();
        }

        public ICollection<ContentItem> GetMessagesContent(string[] contentIds)
        {
            ConcurrentDictionary<string, ContentItem> results = new ConcurrentDictionary<string, ContentItem>();

            Parallel.ForEach(contentIds, id =>
            {
                var item = new ContentItem()
                {
                    FileId = new ObjectId(id),
                    FileBytes = _gridFs.DownloadAsBytes(new ObjectId(id))
                };
                results.TryAdd(id, item);
            });

            return results.Values;
        }

        public async Task<ICollection<DialogEntity>> GetDialogsWithNewMessagesAsync(DialogsNewMessagesParameter parameter)
        {
            var filter = Builders<MessageEntity>.Filter.In(x=>x.DialogId, parameter.DialogIds.Select(x=> new ObjectId(x)))
                            & Builders<MessageEntity>.Filter.AnyEq(x => x.ToParticipants, parameter.Login);

            var grouping = await _collection.Aggregate<MessageEntity>()
                .Match(filter)
                .Group(x => x.DialogId, y => new DialogEntity()
                {
                    Id = y.Key
                })
                .ToListAsync();

            if (grouping == null)
            {
                return new List<DialogEntity>();
            }

            return grouping;
        }

        public async Task<ICollection<DialogsNewMessagesCountResult>> GetDialogNewMessagesCount(
            DialogsNewMessagesParameter parameter)
        {
            if (parameter.DialogIds == null || parameter.DialogIds.Count == 0 ||
                parameter.DialogIds.Any(String.IsNullOrEmpty))
                throw new ArgumentException(nameof(parameter.DialogIds));

            if (String.IsNullOrEmpty(parameter.Login)) throw new ArgumentException(nameof(parameter.Login));

            var newMessagesFilter = Builders<MessageEntity>.Filter.AnyEq(x => x.ToParticipants, parameter.Login)
                                    & Builders<MessageEntity>.Filter.In(x => x.DialogId,
                                        parameter.DialogIds.Select(x => new ObjectId(x)));

            var countResult = await _collection.Aggregate()
                .Match(newMessagesFilter)
                .Group(x => x.DialogId, y => new DialogsNewMessagesCountResult()
                {
                    DialogId = y.Key,
                    NewMessagesCount = y.Count()
                })
                .ToListAsync();

            return countResult;
        }
    }
}
