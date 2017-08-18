using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Funkmap.Common.Data.Mongo.Abstract;
using Funkmap.Messenger.Data.Entities;
using Funkmap.Messenger.Data.Parameters;
using Funkmap.Messenger.Data.Repositories.Abstract;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace Funkmap.Messenger.Data.Repositories
{
    public class DialogRepository : IDialogRepository
    {
        private readonly IMongoCollection<DialogEntity> _collection;
        private readonly IGridFSBucket _gridFs;

        public DialogRepository(IMongoCollection<DialogEntity> collection, IGridFSBucket gridFs)
        {
            _collection = collection;
            _gridFs = gridFs;
        }

        public async Task<ICollection<DialogEntity>> GetUserDialogsAsync(UserDialogsParameter parameter)
        {
            var participantsFilter = Builders<DialogEntity>.Filter.AnyEq(x => x.Participants, parameter.Login);

            var projection = Builders<DialogEntity>.Projection.Exclude(x => x.Messages);

            var dialogs = await _collection
                .Find(participantsFilter)
                .Project<DialogEntity>(projection)
                .Skip(parameter.Skip)
                .Limit(parameter.Take)
                .ToListAsync();
            return dialogs;
        }

        public async Task<ICollection<MessageEntity>> GetDialogMessagesAsync(DialogMessagesParameter parameter)
        {
            var filter = Builders<DialogEntity>.Filter.Eq(x => x.Id, new ObjectId(parameter.DialogId));

            var countProjection = Builders<DialogEntity>.Projection.Include(x => x.MessagesCount);
            var dialog = await _collection.Find(filter).Project<DialogEntity>(countProjection).SingleOrDefaultAsync();
            var messagesCount = dialog.MessagesCount;

            var skip = -(parameter.Skip + parameter.Take);
            var take = skip + messagesCount + parameter.Take;
            if (take <= 0) return new List<MessageEntity>();

            var projection = Builders<DialogEntity>.Projection.Slice(x => x.Messages, skip, take);

            dialog = await _collection.Find(filter).Project<DialogEntity>(projection).SingleOrDefaultAsync();
            var messages = dialog?.Messages;
            return messages;
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

        public async Task AddMessage(string dialogId, MessageEntity message)
        {
            Parallel.ForEach(message.Content, item =>
             {
                 var fileId = _gridFs.UploadFromBytes(item.FileName, item.FileBytes);
                 item.FileId = fileId;
             });

            var update = Builders<DialogEntity>.Update.Push(x => x.Messages, message).Inc(x => x.MessagesCount, 1);
            await _collection.UpdateOneAsync<DialogEntity>(entity => entity.Id == new ObjectId(dialogId), update);
        }

        public async Task<ICollection<string>> GetDialogMembers(string id)
        {
            var filter = Builders<DialogEntity>.Filter.Eq(x => x.Id, new ObjectId(id));
            var projection = Builders<DialogEntity>.Projection.Include(x => x.Participants);
            var dialog = await _collection.Find(filter).Project<DialogEntity>(projection).FirstOrDefaultAsync();
            var members = dialog?.Participants;
            return members;
        }


        public async Task<ICollection<DialogEntity>> GetDialogsWithNewMessages(DialogsWithNewMessagesParameter parameter)
        {
            //db.dialogs.aggregate([
            //{$match:{"prtcpnts.":"rogulenkoko"}}, 
            //{$unwind:"$m"},
            //{$match:{ "m.date": {$gte: ISODate("2017-08-18 20:52:19.913Z")}}}, 
            //{$group:{_id:"_id", m:{$push:"$m"}}}])

            var userMatchFilter = Builders<DialogEntity>.Filter.AnyEq(x => x.Participants, parameter.Login);

            var dateMatchFilter = Builders<MessageUnwinded>.Filter.Gt(x=>x.Message.DateTimeUtc, parameter.LastVisitDate);
            
            var result = await _collection
                .Aggregate<DialogEntity>()
                .Match(userMatchFilter)
                .Unwind<DialogEntity, MessageUnwinded>(x=>x.Messages)
                .Match(dateMatchFilter)
                .Group(x=> x.Id, value => new DialogEntity()
                {
                    Id = value.Key,
                    Messages = value.Select(x=>x.Message).ToList()
                })
                .ToListAsync();

            return result;
        }

        public async Task CreateAsync(DialogEntity item)
        {
            await _collection.InsertOneAsync(item);
        }
    }

}
