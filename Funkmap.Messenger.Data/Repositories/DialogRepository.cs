using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Funkmap.Common.Data.Mongo;
using Funkmap.Messenger.Data.Entities;
using Funkmap.Messenger.Data.Objects;
using Funkmap.Messenger.Data.Parameters;
using Funkmap.Messenger.Data.Repositories.Abstract;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace Funkmap.Messenger.Data.Repositories
{
    public class DialogRepository : MongoRepository<DialogEntity>, IDialogRepository
    {
        private readonly IMongoCollection<MessageEntity> _messagesCollection;

        public DialogRepository(IMongoCollection<DialogEntity> collection, IMongoCollection<MessageEntity> messagesCollection) : base(collection)
        {
            _messagesCollection = messagesCollection;
        }

        public override async Task<DialogEntity> GetAsync(string id)
        {
            var projection = Builders<DialogEntity>.Projection.Exclude(x => x.Avatar);
            var dialog = await _collection.Find(x => x.Id == new ObjectId(id)).Project<DialogEntity>(projection).SingleOrDefaultAsync();
            return dialog;

        }

        public async Task<ICollection<DialogEntity>> GetUserDialogsAsync(string login)
        {
            var projection = Builders<DialogEntity>.Projection.Exclude(x => x.Avatar);
            var filter = Builders<DialogEntity>.Filter.AnyEq(x => x.Participants, login);

            var sortFilter = Builders<DialogEntity>.Sort.Descending(x => x.LastMessageDate);

            var dialogs = await _collection
                .Find(filter)
                .Project<DialogEntity>(projection)
                .Sort(sortFilter)
                .ToListAsync();

            return dialogs;
        }

        public async Task<ICollection<MessageEntity>> GetLastDialogsMessages(string[] dialogIds)
        {
            //db.dialogs.aggregate(
            //    [{$match: { _id: ObjectId("5a304763208c10408cac622e")} },
            //{$lookup: { from: "messages", localField: "_id", foreignField: "d", as: "mes" } },
            //{$project: { _id: 1, message: { $arrayElemAt: ["$mes", 0] } } }
            //])

            //todo перенести эту логику в запрос всех диалогов, чтобы доставать сразу с сообщением

            if (dialogIds == null || dialogIds.Length == 0) throw new ArgumentException(nameof(dialogIds));

            var sort = Builders<DialogLookup>.Sort.Descending(x => x.LastMessages);

            var dialogsFilter = Builders<DialogEntity>.Filter.In(x => x.Id, dialogIds.Select(x => new ObjectId(x)));

            var messages = await _collection.Aggregate()
                .Match(dialogsFilter)
                .Lookup<DialogEntity, MessageEntity, DialogLookup>(_messagesCollection, x => x.Id, x=>x.DialogId, result => result.LastMessages)
                .Sort(sort)
                .Project(x=> new LastDialogMessageResult()
                {
                    DialogId = x.Id,
                    LastMessage = x.LastMessages.Last()
                })
                .ToListAsync();

            if (messages == null) return new List<MessageEntity>();

            return messages.Select(x => x.LastMessage).ToList();
        }

        public async Task<ICollection<DialogEntity>> GetDialogsAvatarsAsync(string[] ids)
        {
            var projection = Builders<DialogEntity>.Projection.Include(x => x.Avatar);
            var filter = Builders<DialogEntity>.Filter.In(x => x.Id, ids.Select(x=>new ObjectId(x)));
            var dialogs = await _collection.Find(filter).Project<DialogEntity>(projection).ToListAsync();
            return dialogs;
        }

        public async Task<DialogEntity> GetDialogAvatarAsync(string id)
        {
            var projection = Builders<DialogEntity>.Projection.Include(x => x.Avatar);
            var filter = Builders<DialogEntity>.Filter.Eq(x => x.Id, new ObjectId(id));
            var dialog = await _collection.Find(filter).Project<DialogEntity>(projection).SingleOrDefaultAsync();
            return dialog;
        }

        public async Task<ICollection<string>> GetDialogMembers(string id)
        {
            var filter = Builders<DialogEntity>.Filter.Eq(x => x.Id, new ObjectId(id));
            var projection = Builders<DialogEntity>.Projection.Include(x => x.Participants);
            var dialog = await _collection.Find(filter).Project<DialogEntity>(projection).FirstOrDefaultAsync();
            var members = dialog?.Participants;
            return members;
        }

        public async Task UpdateLastMessageDate(UpdateLastMessageDateParameter parameter)
        {
            if(String.IsNullOrEmpty(parameter.DialogId)) throw new ArgumentException(nameof(parameter.DialogId));
            if(parameter.Date == DateTime.MinValue) throw new ArgumentException(nameof(parameter.DialogId));

            var update = Builders<DialogEntity>.Update.Set(x => x.LastMessageDate, parameter.Date);
            await _collection.UpdateOneAsync(x => x.Id == new ObjectId(parameter.DialogId), update);
        }

        public async Task<ObjectId> CreateAndGetIdAsync(DialogEntity item)
        {
            if(item == null || item.Participants.Count < 2) throw new ArgumentException(nameof(item));
            await _collection.InsertOneAsync(item);

            return item.Id;

        }

        public override async Task UpdateAsync(DialogEntity entity)
        {
            var filter = Builders<DialogEntity>.Filter.Eq(x => x.Id, entity.Id);

            await _collection.ReplaceOneAsync(filter, entity);
        }

        public async Task<bool> CheckDialogExist(List<string> particpants)
        {
            var filter = Builders<DialogEntity>.Filter.All(x => x.Participants, particpants);
            var projection = Builders<DialogEntity>.Projection.Include(x => x.Id);
            var existingDialog = await _collection.Find(filter).Project(projection).Limit(1).ToListAsync();
            if (existingDialog == null || existingDialog.Count == 0) return false;
            return true;
        }

        public async Task<bool> CheckDialogExist(string dialogId)
        {
            var filter = Builders<DialogEntity>.Filter.Eq(x => x.Id, new ObjectId(dialogId));
            var projection = Builders<DialogEntity>.Projection.Include(x => x.Id);
            var existingDialog = await _collection.Find(filter).Project(projection).Limit(1).ToListAsync();
            if (existingDialog == null || existingDialog.Count == 0) return false;
            return true;

        }
    }

}
