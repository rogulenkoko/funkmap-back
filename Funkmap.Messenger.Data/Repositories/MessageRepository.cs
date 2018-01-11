using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Features.AttributeFilters;
using Funkmap.Common.Abstract;
using Funkmap.Messenger.Data.Objects;
using Funkmap.Messenger.Data.Parameters;
using Funkmap.Messenger.Data.Repositories.Abstract;
using Funkmap.Messenger.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Funkmap.Messenger.Data.Repositories
{
    public class MessageRepository :IMessageRepository
    {
        private readonly IMongoCollection<MessageEntity> _collection;
        private readonly IFileStorage _fileStorage;

        public MessageRepository(IMongoCollection<MessageEntity> collection,
            [KeyFilter(MessengerCollectionNameProvider.MessengerStorage)]IFileStorage fileStorage)
        {
            _collection = collection;
            _fileStorage = fileStorage;
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

            
                
            return messages.Reverse().ToList();
        }

        public ICollection<ContentItem> GetMessagesContent(string[] contentIds)
        {
            ConcurrentDictionary<string, ContentItem> results = new ConcurrentDictionary<string, ContentItem>();

            Parallel.ForEach(contentIds, id =>
            {
                var item = new ContentItem()
                {
                    FileId = id,
                    FileBytes = _fileStorage.DownloadAsBytesAsync(id).GetAwaiter().GetResult()
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

        public async Task AddMessage(MessageEntity message)
        {
            await _collection.InsertOneAsync(message);
        }
    }
}
