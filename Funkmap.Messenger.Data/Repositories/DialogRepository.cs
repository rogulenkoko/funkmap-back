using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Funkmap.Messenger.Data.Entities;
using Funkmap.Messenger.Data.Parameters;
using Funkmap.Messenger.Data.Repositories.Abstract;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Funkmap.Messenger.Data.Repositories
{
    public class DialogRepository : IDialogRepository
    {
        private readonly IMongoCollection<DialogEntity> _collection;

        public DialogRepository(IMongoCollection<DialogEntity> collection)
        {
            _collection = collection;
        }
        
        public async Task<ICollection<DialogEntity>> GetUserDialogsAsync(UserDialogsParameter parameter)
        {
            var projection = Builders<DialogEntity>.Projection.Exclude(x => x.Messages);

            var dialogs = await _collection
                .Find(dialog => dialog.Participants.Any(participant => participant == parameter.Login))
                .Project<DialogEntity>(projection)
                .Skip(parameter.Skip)
                .Limit(parameter.Take)
                .ToListAsync();
            return  dialogs;
        }

        public async Task<ICollection<MessageEntity>> GetDialogMessagesAsync(DialogMessagesParameter parameter)
        {
            var filter = Builders<DialogEntity>.Filter.Eq(x => x.Id, new ObjectId(parameter.DialogId));

            var countProjection = Builders<DialogEntity>.Projection.Include(x => x.MessagesCount);
            var dialog = await _collection.Find(filter).Project<DialogEntity>(countProjection).SingleOrDefaultAsync();
            var messagesCount = dialog.MessagesCount;

            var skip = -(parameter.Skip + parameter.Take);
            var take = skip + messagesCount + parameter.Take;
            if(take <= 0) return new List<MessageEntity>();

            var projection = Builders<DialogEntity>.Projection.Slice(x => x.Messages, skip, take);
            
            dialog = await _collection.Find(filter).Project<DialogEntity>(projection).SingleOrDefaultAsync();
            var messages = dialog?.Messages;
            return messages;
        }

        public async Task AddMessage(string dialogId, MessageEntity message)
        {
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

        public async Task CreateAsync(DialogEntity item)
        {
            await _collection.InsertOneAsync(item);
        }
    }
}
