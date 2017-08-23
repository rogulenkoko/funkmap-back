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

        public async Task<MessageEntity> GetLastDialogMessage(string dialogId)
        {
            if(String.IsNullOrEmpty(dialogId)) throw new ArgumentException(nameof(dialogId));

            var sort = Builders<MessageEntity>.Sort.Descending(x => x.Id);
            var message = await _collection.Find(x => x.DialogId == new ObjectId(dialogId)).Sort(sort).Limit(1).FirstOrDefaultAsync();
            return message;
        }

        public async Task<ICollection<MessageEntity>> GetDialogMessagesAsync(DialogMessagesParameter parameter)
        {

            if (String.IsNullOrEmpty(parameter.UserLogin) || String.IsNullOrEmpty(parameter.DialogId))
            {
                throw new ArgumentException(nameof(parameter));
            }

            var dialogFilter = Builders<MessageEntity>.Filter.Eq(x => x.DialogId, new ObjectId(parameter.DialogId));
            var messageProjection = Builders<MessageEntity>.Projection
                .Exclude(x => x.ParticipantsWhoRead);

            var sort = Builders<MessageEntity>.Sort.Descending(x => x.Id);

            ICollection<MessageEntity> messages = await _collection.Find(dialogFilter).Project<MessageEntity>(messageProjection)
                .Sort(sort)
                .Skip(parameter.Skip)
                .Limit(parameter.Take)
                .ToListAsync();

            var readFilter = Builders<MessageEntity>.Filter.In(x => x.Id, messages.Select(x => x.Id))
                            & Builders<MessageEntity>.Filter.AnyNe(x=>x.ParticipantsWhoRead, parameter.UserLogin)
                            & Builders<MessageEntity>.Filter.Ne(x=> x.Sender, parameter.UserLogin);

            var update = Builders<MessageEntity>.Update.AddToSet(x => x.ParticipantsWhoRead, parameter.UserLogin).Set(x=>x.IsRead, true);

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

        public async Task<int> GetDialogsWithNewMessagesCountAsync(DialogsNewMessagesParameter parameter)
        {
            var filter = Builders<MessageEntity>.Filter.In(x=>x.DialogId, parameter.DialogIds.Select(x=> new ObjectId(x)))
                            & Builders<MessageEntity>.Filter.AnyNe(x => x.ParticipantsWhoRead, parameter.Login);

            var grouping = await _collection.Aggregate<MessageEntity>()
                .Match(filter)
                .Group(x => x.DialogId, y => new DialogEntity()
                {
                    Id = y.Key
                })
                .ToListAsync();

            if (grouping == null)
            {
                return 0;
            }

            return grouping.Count;
        }

        public async Task<ICollection<DialogsNewMessagesCountResult>> GetDialogNewMessagesCount(
            DialogsNewMessagesParameter parameter)
        {
            if (parameter.DialogIds == null || parameter.DialogIds.Count == 0 ||
                parameter.DialogIds.Any(String.IsNullOrEmpty))
                throw new ArgumentException(nameof(parameter.DialogIds));

            if (String.IsNullOrEmpty(parameter.Login)) throw new ArgumentException(nameof(parameter.Login));

            var newMessagesFilter = Builders<MessageEntity>.Filter.AnyNe(x => x.ParticipantsWhoRead, parameter.Login)
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
